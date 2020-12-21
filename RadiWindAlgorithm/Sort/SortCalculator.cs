/*  Copyright 2020 RadiRhino-秋水, 笑里追风. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using Grasshopper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadiWindAlgorithm.Sort
{
    public static class SortCalculator
    {
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

        #region NumberTolerancePartion
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

        /// <summary>
        /// Dispatch the List
        /// </summary>
        /// <typeparam name="T">Value Type</typeparam>
        /// <param name="sortableTree">Value in double list.</param>
        /// <param name="indexs">out Index.</param>
        /// <returns></returns>
        public static List<List<T>> DispatchIt<T>(List<List<SortableItem<T>>> sortableTree, out List<List<int>> indexs)
        {
            List<List<T>> outList = new List<List<T>>();
            indexs = new List<List<int>>();

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
                indexs.Add(indexList);
            }

            return outList;
        }
    }
}
