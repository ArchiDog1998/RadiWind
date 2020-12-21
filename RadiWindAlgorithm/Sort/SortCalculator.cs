/*  Copyright 2020 RadiRhino-秋水, 笑里追风. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using Grasshopper;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadiWindAlgorithm.Sort
{
    public static class SortCalculator
    {
        #region  SortPointInAxis
        public static List<Point3d> SortPointInAxisForPython(List<Point3d> values, int type, Plane plane, out List<int> indexes)
        {
            List<Point3d> waitToSort = PlaneServer.PlaneCoordinate(plane, values);
            return SortPointInAxis(waitToSort, type, out indexes);
        }

        public static List<Point3d> SortPointInAxis(List<Point3d> values, int type, out List<int> indexes)
        {
            if (type < 0 || type > 2)
                throw new ArgumentOutOfRangeException("type", "type must be in 0-2!");

            var needToSorItems = GetSortableItems(values);
            needToSorItems.Sort((x, y) => x.Value[type].CompareTo(y.Value[type]));
            return DispatchIt(needToSorItems, out indexes);
        }

        #endregion

        #region NumberTolerancePartion

        public static DataTree<double> NumberTolerancePartionSortForPython(DataTree<double> values, double tolerance, out DataTree<int> indexes)
        {
            DataTree<double> outTree = new DataTree<double>();
            indexes = new DataTree<int>();
            for (int i = 0; i < values.BranchCount; i++)
            {
                List<List<int>> resultindexes;
                List<List<double>> resultTree = NumberTolerancePartionSort(values.Branches[i], tolerance, out resultindexes);
                outTree.SetToDataTree(resultTree, i);
                indexes.SetToDataTree(resultindexes, i);
            }
            return outTree;
        }

        public static List<List<double>> NumberTolerancePartionSort(List<double> values, double tolerance, out List<List<int>> indexes)
        {
            return NumberTolerancePartionSort<double>(values, (x) => x, tolerance, out indexes);
        }


        /// <summary>
        /// Partion sort with tolerance
        /// </summary>
        /// <typeparam name="T">ValueType</typeparam>
        /// <param name="values">ValueList</param>
        /// <param name="getDouble">how to know the key.</param>
        /// <param name="tolerance">tolerance in double</param>
        /// <param name="indexs">out in index.</param>
        /// <returns></returns>
        public static List<List<T>> NumberTolerancePartionSort<T>(List<T> values, Func<T, double> getDouble, double tolerance, out List<List<int>> indexs)
        {
            List<List<SortableItem<T>>> sortableTree = NumberTolerancePartionSort<T>(values, getDouble, tolerance);
            return DispatchIt<T>(sortableTree, out indexs);
        }

        /// <summary>
        /// Partion sort with tolerance
        /// </summary>
        /// <typeparam name="T">ValueType</typeparam>
        /// <param name="values">ValueList</param>
        /// <param name="getDouble">how to know the key.</param>
        /// <param name="tolerance">tolerance in double</param>
        /// <returns></returns>
        public static List<List<SortableItem<T>>> NumberTolerancePartionSort<T>(List<T> values, Func<T, double> getDouble, double tolerance)
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

        #region NearlestPointSort

        public static List<Point3d> NearlestPointSortForPython(List<Point3d> inputPoints, int index, out List<int> indexes)
        {
            List<SortableItem<Point3d>> result = NearlestPointSort(GetSortableItems(inputPoints), index);
            return DispatchIt<Point3d>(result, out indexes);
        }

        public static List<SortableItem<Point3d>> NearlestPointSort(List<SortableItem<Point3d>> needToCalculateItems, int index)
        {
            List<SortableItem<Point3d>> outItems = new List<SortableItem<Point3d>>() { needToCalculateItems[index] };
            needToCalculateItems.RemoveAt(index);

            while (needToCalculateItems.Count > 0)
            {
                Point3d flagPt = outItems[outItems.Count - 1].Value;

                //Find the point that is nearlist to the flagPt.
                SortableItem<Point3d> nearlistItem = needToCalculateItems[0];
                double minDistance = flagPt.DistanceTo(nearlistItem.Value);
                for (int i = 1; i < needToCalculateItems.Count; i++)
                {
                    double distance = flagPt.DistanceTo(needToCalculateItems[i].Value);
                    if (distance < minDistance)
                    {
                        nearlistItem = needToCalculateItems[i];
                        minDistance = distance;
                    }
                }

                //Remove and Add.
                outItems.Add(nearlistItem);
                needToCalculateItems.Remove(nearlistItem);
            }
            return outItems;
        }
        #endregion

        #region Converter
        /// <summary>
        /// Get the SortableItems.
        /// </summary>
        /// <typeparam name="T">Value Type</typeparam>
        /// <param name="values">Value in list</param>
        /// <returns></returns>
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
        /// Dispatch the List
        /// </summary>
        /// <typeparam name="T">Value Type</typeparam>
        /// <param name="sortableTree">Value in double list.</param>
        /// <param name="indexes">out Index.</param>
        /// <returns></returns>
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
