/*  Copyright 2020 RadiRhino-秋水, 笑里追风. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using Grasshopper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace RadiWind.Tests
{
    public static class TestsHelper
    {

        #region Is List Equal
        /// <summary>
        /// Find out is List equal
        /// </summary>
        /// <param name="expectList">list a</param>
        /// <param name="actualList">list b</param>
        /// <returns>is equal</returns>
        public static bool IsListEqual<T>(List<T> expectList, List<T> actualList, Func<T, T, bool> equalFunc)
        {
            if (expectList.Count != actualList.Count)
                return false;

            for (int i = 0; i < expectList.Count; i++)
            {
                if (!equalFunc.Invoke(expectList[i], actualList[i]))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Find out is DoubleList equal
        /// </summary>
        /// <param name="expectList">double list a</param>
        /// <param name="actualList">double list b</param>
        /// <returns>is equal</returns>
        public static bool IsDoubleListEqual<T>(List<List<T>> expectList, List<List<T>> actualList, Func<T, T, bool> equalFunc)
        {
            if (expectList.Count != actualList.Count)
                return false;

            for (int i = 0; i < expectList.Count; i++)
            {
                if (!IsListEqual(expectList[i], actualList[i], equalFunc))
                    return false;
            }

            return true;
        }
        #endregion

        /// <summary>
        /// Check the DataTree<int> is Equal
        /// </summary>
        /// <param name="expectTree">expectTree</param>
        /// <param name="actualTree">actualTree</param>
        /// <returns>is Equal</returns>
        public static bool IsDataTreeEqual<T>(DataTree<T> expectTree, DataTree<T> actualTree, Func<T, T, bool> equalFunc)
        {
            //Check Path Count
            if (expectTree.BranchCount != actualTree.BranchCount)
                return false;

            //Check is Path equal.
            for (int i = 0; i < expectTree.BranchCount; i++)
            {
                if (expectTree.Path(i) != actualTree.Path(i))
                    return false;
            }

            List<T> expectDatas = expectTree.AllData();
            List<T> actualDatas = actualTree.AllData();

            //Check Data Count.
            if (expectDatas.Count != actualDatas.Count)
                return false;

            //Check is Data equal.
            for (int j = 0; j < expectDatas.Count; j++)
            {
                if (!equalFunc( expectDatas[j], actualDatas[j]))
                    return false;
            }

            return true;
        }
    }
}
