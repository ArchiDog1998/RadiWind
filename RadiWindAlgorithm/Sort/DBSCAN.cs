/*  Copyright 2021 RadiRhino-秋水. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using Grasshopper;
using Grasshopper.Kernel.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class DBSCAN_Algorithm
    {
        public List<DBSCAN_Tensor> Tensors { get; private set; }
        public DBSCAN_PointDistanceTable DistanceTable { get; private set; }
        public List<double> Radius { get; private set; }
        public int MinCount { get; private set; }

        private List<DBSCAN_Cluster> Clusters { get; set; }
        private List<DBSCAN_Tensor> Noise { get; set; }

        public DataTree<double> ClustersTree 
        {
            get
            {
                DataTree<double> dataTree = new DataTree<double>();
                for (int i = 0; i < Clusters.Count; i++)
                {
                    foreach (DBSCAN_Tensor tensor in Clusters[i].Points)
                    {
                        List<int> pathInt = new List<int>() { i };
                        pathInt.AddRange(tensor.Path.Indices);
                        dataTree.AddRange(tensor.Data, new GH_Path(pathInt.ToArray()));
                    }
                }
                return dataTree;
            }
        }

        public DataTree<double> NoiseTree
        {
            get
            {
                DataTree<double> dataTree = new DataTree<double>();

                foreach (DBSCAN_Tensor tensor in Noise)
                {
                    dataTree.AddRange(tensor.Data, tensor.Path);
                }

                return dataTree;
            }
        }

        public DBSCAN_Algorithm(DataTree<double> inputValues, List<double> radiuses, int minCount)
        {
            Tensors = new List<DBSCAN_Tensor>();
            Radius = radiuses;
            MinCount = minCount;

            for (int i = 0; i < inputValues.BranchCount; i++)
            {
                Tensors.Add(new DBSCAN_Tensor(inputValues.Branch(i), inputValues.Path(i), i));
            }

            DistanceTable = new DBSCAN_PointDistanceTable(this);

            this.Clusters = new List<DBSCAN_Cluster>();
            this.Noise = new List<DBSCAN_Tensor>();

            DBSCAN_Tensor calculatePoint = GetFirstUnvisitedPoint();

            while (calculatePoint != null)
            {
                DBSCAN_Tensor[] neighborhood;
                if (DistanceTable.IsCorePoint(calculatePoint, out neighborhood))
                {
                    Clusters.Add(new DBSCAN_Cluster(this.DistanceTable, calculatePoint, neighborhood));
                }
                else
                {
                    Noise.Add(calculatePoint);
                }
                calculatePoint = GetFirstUnvisitedPoint();
            }
        }


        public DBSCAN_Tensor GetFirstUnvisitedPoint()
        {
            foreach (var point in Tensors)
            {
                if(point.IsVisited == false)
                {
                    point.IsVisited = true;
                    return point;
                }
            }
            return null;
        }
    }

    public class DBSCAN_Cluster
    {

        public HashSet<DBSCAN_Tensor> Points { get; private set; } 
        public DBSCAN_Cluster(DBSCAN_PointDistanceTable table, DBSCAN_Tensor point, DBSCAN_Tensor[] neighborhood)
        {
            Points = new HashSet<DBSCAN_Tensor>() { point };

            //Get First Queue
            HashSet<DBSCAN_Tensor> waitTensor = new HashSet<DBSCAN_Tensor>();
            foreach (var tensor in neighborhood)
            {
                if (tensor.IsVisited == false)
                    waitTensor.Add(tensor);
            }
            

            while(waitTensor.Count > 0)
            {
                waitTensor = OneStep(table, waitTensor);
            }
            
        }

        private HashSet<DBSCAN_Tensor> OneStep(DBSCAN_PointDistanceTable table, HashSet<DBSCAN_Tensor> input)
        {
            HashSet<DBSCAN_Tensor> nextStep = new HashSet<DBSCAN_Tensor>();
            foreach (var tensor in input)
            {

                DBSCAN_Tensor[] neighborhood;
                if (table.IsCorePoint(tensor, out neighborhood))
                {
                    foreach (var subtensor in neighborhood)
                    {
                        if (subtensor.IsVisited == false)
                            nextStep.Add(subtensor);
                    }
                }
                Points.Add(tensor);
            }
            return nextStep;
        }
    }

    /// <summary>
    /// this class is to remember the distance, and check is point core.
    /// </summary>
    public class DBSCAN_PointDistanceTable
    {
        public DBSCAN_Algorithm Algorithm { get; private set; }

        private bool[] _isClose;

        /// <summary>
        /// Return the distance between two points.
        /// </summary>
        /// <param name="point1">the first point.</param>
        /// <param name="point2">the second point.</param>
        /// <returns></returns>
        public bool this[DBSCAN_Tensor point1, DBSCAN_Tensor point2]
        {
            get
            {
                return this[point1.Index, point2.Index];
            }
        }

        private bool this[int index1, int index2]
        {
            get
            {
                int max = Math.Max(index1, index2);
                int min = Math.Min(index1, index2);

                if (min == max || max >= Algorithm.Tensors.Count) return false;

                int index = min;
                for (int i = 0; i < max; i++) index += i;
                return _isClose[index];
            }
        }

        public DBSCAN_PointDistanceTable(DBSCAN_Algorithm dbscan)
        {
            Algorithm = dbscan;

            List<bool> distance = new List<bool>();
            for (int i = 1; i < Algorithm.Tensors.Count; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    distance.Add(Algorithm.Tensors[i].IsCloseTo(Algorithm.Tensors[j], dbscan.Radius));
                }
            }
            _isClose = distance.ToArray();
        }

        /// <summary>
        /// Check the point is core point.
        /// </summary>
        /// <param name="point"></param>
        /// <param name="neighborhood"></param>
        /// <returns></returns>
        public bool IsCorePoint(DBSCAN_Tensor point, out DBSCAN_Tensor[] neighborhood)
        {
            point.IsVisited = true;

            int count = 0;
            List<DBSCAN_Tensor> neighRelay = new List<DBSCAN_Tensor>();

            foreach (DBSCAN_Tensor pt in Algorithm.Tensors)
            {
                if (pt == point) continue;
                if(this[pt, point])
                {
                    count++;
                    neighRelay.Add(pt);
                }
            }

            neighborhood = neighRelay.ToArray();
            return count >= Algorithm.MinCount;
        }
    }

    public class DBSCAN_Tensor
    {
        public List<double> Data { get; private set; }
        public double this[int index] 
        {
            get
            {
                return Data[index];
            }
            private set
            {

            }
        } 
        public GH_Path Path { get; private set; }
        public bool IsVisited { get; set; }
        private int Size
        {
            get
            {
                return Data.Count;
            }
            set
            {

            }
        }
        public int Index { get; private set; }
        public DBSCAN_Tensor(List<double> datas, GH_Path path, int index)
        {
            IsVisited = false;
            Data = datas;
            Path = path;
            Index = index;
        }

        public bool IsCloseTo(DBSCAN_Tensor otherPoint, List<double> radiuses)
        {
            if (otherPoint.Size != this.Size || this.Size!=radiuses.Count)
                throw new Exception("Two Point's Size is not the same!");

            for (int i = 0; i < Size; i++)
            {
                if (Math.Abs(this[i] - otherPoint[i]) > radiuses[i])
                    return false;
                
            }
            return true;
        }
    }


}
