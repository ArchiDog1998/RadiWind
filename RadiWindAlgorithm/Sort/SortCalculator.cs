/*  Copyright 2020 RadiRhino-秋水, 笑里追风. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using Grasshopper;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadiWindAlgorithm.Sort
{
    public static class SortCalculator
    {
        #region  SortPointInAxis
        /// <summary>
        /// Sort point by one axis.
        /// </summary>
        /// <param name="points">points to input</param>
        /// <param name="axisType">an integer for type between x, y, z.</param>
        /// <param name="indexes">the sorted points' index</param>
        /// <returns>the sorted points.</returns>
        [Pythonable]
        public static List<Point3d> SortPointInAxis(List<Point3d> points, int axisType, Plane plane, out List<int> indexes)
        {
            List<Point3d> waitToSort = PlaneServer.PlaneCoordinate(plane, points);
            return SortPointInAxis(waitToSort, axisType, out indexes);
        }

        /// <summary>
        /// Sort point by one axis.
        /// </summary>
        /// <param name="points">points to input</param>
        /// <param name="axisType">an integer for type between x, y, z.</param>
        /// <param name="indexes">the sorted points' index</param>
        /// <returns>the sorted points.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static List<Point3d> SortPointInAxis(List<Point3d> points, int axisType, out List<int> indexes)
        {
            if (axisType < 0 || axisType > 2)
                throw new ArgumentOutOfRangeException("type", "type must be in 0-2!");

            var needToSorItems = GetSortableItems(points);
            needToSorItems.Sort((x, y) => x.Value[axisType].CompareTo(y.Value[axisType]));
            return DispatchIt(needToSorItems, out indexes);
        }

        #endregion

        #region NumberTolerancePartion
        /// <summary>
        /// Sort number and partition them with Tolerance.
        /// </summary>
        /// <param name="numberTree">the number Tree that inputs in.</param>
        /// <param name="tolerance">tolerance</param>
        /// <param name="indexes">the sorted numbers' indexes.</param>
        /// <returns>the sorted numbers.</returns>
        [Pythonable]
        public static DataTree<double> NumberTolerancePartitionSort(DataTree<double> numberTree, double tolerance, out DataTree<int> indexes)
        {
            DataTree<double> outTree = new DataTree<double>();
            indexes = new DataTree<int>();
            for (int i = 0; i < numberTree.BranchCount; i++)
            {
                List<List<int>> resultindexes;
                List<List<double>> resultTree = NumberTolerancePartitionSort(numberTree.Branches[i], tolerance, out resultindexes);
                outTree.SetDataIntoDataTree(resultTree, i);
                indexes.SetDataIntoDataTree(resultindexes, i);
            }
            return outTree;
        }

        /// <summary>
        /// Sort number and partition them with Tolerance.
        /// </summary>
        /// <param name="numberList">list of numbers to input.</param>
        /// <param name="tolerance">tolerance</param>
        /// <param name="indexes">the sorted numbers' indexes.</param>
        /// <returns>the sorted numbers.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static List<List<double>> NumberTolerancePartitionSort(List<double> numberList, double tolerance, out List<List<int>> indexes)
        {
            return NumberTolerancePartitionSort<double>(numberList, (x) => x, tolerance, out indexes);
        }

        /// <summary>
        /// Partion sort with tolerance in double.
        /// </summary>
        /// <typeparam name="T">ValueType</typeparam>
        /// <param name="values">ValueList</param>
        /// <param name="getDouble">how to know the key.</param>
        /// <param name="tolerance">tolerance in double</param>
        /// <param name="indexs">out in index.</param>
        /// <returns>sorted numbers.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static List<List<T>> NumberTolerancePartitionSort<T>(List<T> values, Func<T, double> getDouble, double tolerance, out List<List<int>> indexs)
        {
            List<List<SortableItem<T>>> sortableTree = NumberTolerancePartitionSort<T>(values, getDouble, tolerance);
            return DispatchIt<T>(sortableTree, out indexs);
        }

        /// <summary>
        /// Partion sort with tolerance in double.
        /// </summary>
        /// <typeparam name="T">ValueType</typeparam>
        /// <param name="values">ValueList</param>
        /// <param name="getDouble">how to know the key.</param>
        /// <param name="tolerance">tolerance in double</param>
        /// <returns>sorted numbers in sortableItem.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static List<List<SortableItem<T>>> NumberTolerancePartitionSort<T>(List<T> values, Func<T, double> getDouble, double tolerance)
        {
            //Sort it.
            List<SortableItem<T>> sortableItems = GetSortableItems<T>(values);
            sortableItems.Sort((x, y) =>
            {
                return getDouble.Invoke(x.Value).CompareTo(getDouble.Invoke(y.Value));
            });

            //Participate it
            List<List<SortableItem<T>>> sortableTree = new List<List<SortableItem<T>>>() { new List<SortableItem<T>>() { sortableItems[0] } };
            for (int i = 1; i < sortableItems.Count; i++)
            {
                //if the gap bigger than tolerance
                if (getDouble(sortableItems[i].Value) - getDouble(sortableItems[i - 1].Value) >= tolerance)
                {
                    List<SortableItem<T>> list = new List<SortableItem<T>>() { sortableItems[i] };
                    sortableTree.Add(list);
                }
                else
                {
                    sortableTree[sortableTree.Count - 1].Add(sortableItems[i]);
                }
            }
            return sortableTree;
        }
        #endregion

        #region NearlestPointSortByIndex
        /// <summary>
        /// Sort the nearlest point.the first point is selected by index.
        /// </summary>
        /// <param name="inputPoints">points list</param>
        /// <param name="index">first point's index.</param>
        /// <param name="indexes">sorted points' indexes.</param>
        /// <returns>sorted points</returns>
        [Pythonable]
        public static List<Point3d> NearlestPointSortByIndex(List<Point3d> inputPoints, int index, out List<int> indexes)
        {
            List<SortableItem<Point3d>> result = NearlestPointSortByIndex(GetSortableItems(inputPoints), index);
            return DispatchIt<Point3d>(result, out indexes);
        }

        /// <summary>
        /// Sort the nearlest point.the first point is selected by index.
        /// </summary>
        /// <param name="inpuSortableItems">sortable items' list</param>
        /// <param name="index">first point's index.</param>
        /// <returns>sorted sortable items.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static List<SortableItem<Point3d>> NearlestPointSortByIndex(List<SortableItem<Point3d>> inpuSortableItems, int index)
        {
            List<SortableItem<Point3d>> outItems = new List<SortableItem<Point3d>>() { inpuSortableItems[index] };
            inpuSortableItems.RemoveAt(index);

            while (inpuSortableItems.Count > 0)
            {
                Point3d flagPt = outItems[outItems.Count - 1].Value;

                //Find the point that is nearlist to the flagPt.
                SortableItem<Point3d> nearlistItem = inpuSortableItems[0];
                double minDistance = flagPt.DistanceTo(nearlistItem.Value);
                for (int i = 1; i < inpuSortableItems.Count; i++)
                {
                    double distance = flagPt.DistanceTo(inpuSortableItems[i].Value);
                    if (distance < minDistance)
                    {
                        nearlistItem = inpuSortableItems[i];
                        minDistance = distance;
                    }
                }

                //Remove and Add.
                outItems.Add(nearlistItem);
                inpuSortableItems.Remove(nearlistItem);
            }
            return outItems;
        }
        #endregion

        #region Converter for SortableItem
        /// <summary>
        /// Get the SortableItems.
        /// </summary>
        /// <typeparam name="T">Value Type</typeparam>
        /// <param name="values">Value in list</param>
        /// <returns>sortableItems' list</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static List<SortableItem<T>> GetSortableItems<T>(List<T> values)
        {
            List<SortableItem<T>> outList = new List<SortableItem<T>>();
            for (int i = 0; i < values.Count; i++)
            {
                outList.Add(new SortableItem<T>(i, values[i]));
            }
            return outList;
        }

        /// <summary>
        /// Dispatch the list form sortableItem to value and index.
        /// </summary>
        /// <typeparam name="T">Value Type</typeparam>
        /// <param name="sortableTree">Value in double list.</param>
        /// <param name="indexes">out Index.</param>
        /// <returns>values' double list.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static List<List<T>> DispatchIt<T>(List<List<SortableItem<T>>> sortableTree, out List<List<int>> indexes)
        {
            List<List<T>> outList = new List<List<T>>();
            indexes = new List<List<int>>();

            foreach (var sortableList in sortableTree)
            {
                List<T> valueList = new List<T>();
                List<int> indexList = new List<int>();

                foreach (var item in sortableList)
                {
                    valueList.Add(item.Value);
                    indexList.Add(item.Index);
                }

                outList.Add(valueList);
                indexes.Add(indexList);
            }

            return outList;
        }

        /// <summary>
        /// Dispatch the list form sortableItem to value and index.
        /// </summary>
        /// <typeparam name="T">Value type</typeparam>
        /// <param name="sortableList">input sortableItems' list</param>
        /// <param name="indexes">dispatched indexes.</param>
        /// <returns>dispatched values.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static List<T> DispatchIt<T>(List<SortableItem<T>> sortableList, out List<int> indexes)
        {
            indexes = new List<int>();
            List<T> values = new List<T>();
            foreach (var item in sortableList)
            {
                indexes.Add(item.Index);
                values.Add(item.Value);
            }
            return values;
        }
        #endregion
    }
}
