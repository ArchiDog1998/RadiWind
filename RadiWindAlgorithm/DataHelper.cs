﻿/*  Copyright 2020 RadiRhino-秋水, 笑里追风. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using Grasshopper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadiWindAlgorithm
{
    public static class DataHelper
    {

        #region Double List to DataTree
        public static DataTree<T> SetToDataTree<T>(this DataTree<T> beforeTree, List<List<T>> datas, int runCount = 0)
        {
            return SetToDataTree<T>(datas, runCount, beforeTree);
        }

        /// <summary>
        /// Transform datatree from double list, also can add to Datatree given before.
        /// </summary>
        /// <typeparam name="T">Value Type</typeparam>
        /// <param name="datas">Value as double list</param>
        /// <returns></returns>
        public static DataTree<T> SetToDataTree<T> (List<List<T>> datas, int runCount = 0, DataTree<T> beforeTree = null)
        {
            beforeTree = beforeTree ?? new DataTree<T>();
            for (int i = 0; i < datas.Count; i++)
            {
                foreach (var item in datas[i])
                {
                    beforeTree.Add(item, new Grasshopper.Kernel.Data.GH_Path(runCount, i));
                }
            }
            return beforeTree;

        }
        #endregion

    }
}
