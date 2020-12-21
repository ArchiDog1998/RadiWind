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

namespace RadiWind
{
    public static class DataHelper
    {
        /// <summary>
        /// Transform datatree from double list.
        /// </summary>
        /// <typeparam name="T">Value Type</typeparam>
        /// <param name="datas">Value as double list</param>
        /// <returns></returns>
        public static DataTree<T> SetToDataTree<T> (List<List<T>> datas, int runCount = 0)
        {
            DataTree<T> outTree = new DataTree<T>();
            for (int i = 0; i < datas.Count; i++)
            {
                foreach (var item in datas[i])
                {
                    outTree.Add(item, new Grasshopper.Kernel.Data.GH_Path(runCount, i));
                }
            }
            return outTree;

        }
    }
}
