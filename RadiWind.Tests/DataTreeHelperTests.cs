/*  Copyright 2020 Radino 秋水. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using RadiWindAlgorithm;
using Grasshopper;
using Grasshopper.Kernel.Data;

namespace RadiWind.Tests
{
    /// <summary>
    /// Summary description for DataTreeHelperTests
    /// </summary>
    [TestClass]
    public class DataTreeHelperTests
    {
        [TestMethod]
        public void SetDataIntoDataTreeNotEmptyTreeTest()
        {
            DataTree<int> actualTree = new DataTree<int>();
            actualTree.Add(-1, new GH_Path(0));
            List<List<int>> addedList;
            DataTree<int> expectTree = GetExpectTree(out addedList, actualTree, 1);

            actualTree.SetDataIntoDataTree(addedList, 1);

            Assert.IsTrue(AreDataTreeEqual(expectTree, actualTree));
        }

        [TestMethod]
        public void SetDataIntoDataTreeEmptyTreeTest()
        {
            DataTree<int> actualTree = new DataTree<int>();
            List<List<int>> addedList;
            DataTree<int> expectTree = GetExpectTree(out addedList, actualTree, 0);

            actualTree.SetDataIntoDataTree(addedList, 0);

            Assert.IsTrue(AreDataTreeEqual(expectTree, actualTree));
        }

        #region Check DataTree helper.
        /// <summary>
        /// Get a Try Data Set.
        /// </summary>
        /// <param name="addedList"></param>
        /// <param name="acutalTree"></param>
        /// <returns></returns>
        private DataTree<int> GetExpectTree(out List<List<int>> addedList, DataTree<int> acutalTree, int runcount)
        {
            addedList = new List<List<int>>()
            {
                new List<int>(){0, 1, 2},
                new List<int>(){3, 4, 5},
                new List<int>(){6, 7, 8}
            };

            DataTree<int> expectTree = acutalTree;
            expectTree.AddRange(new List<int>() { 0, 1, 2 }, new GH_Path(runcount, 0));
            expectTree.AddRange(new List<int>() { 3, 4, 5 }, new GH_Path(runcount, 1));
            expectTree.AddRange(new List<int>() { 6, 7, 8 }, new GH_Path(runcount, 2));

            return expectTree;
        }

        /// <summary>
        /// Check the DataTree<int> is Equal
        /// </summary>
        /// <param name="expectTree">expectTree</param>
        /// <param name="actualTree">actualTree</param>
        /// <returns>is Equal</returns>
        private bool AreDataTreeEqual(DataTree<int> expectTree, DataTree<int> actualTree)
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

            List<int> expectDatas = expectTree.AllData();
            List<int> actualDatas = actualTree.AllData();

            //Check Data Count.
            if (expectDatas.Count != actualDatas.Count)
                return false;

            //Check is Data equal.
            for (int j = 0; j < expectDatas.Count; j++)
            {
                if (expectDatas[j] != actualDatas[j])
                    return false;
            }

            return true;
        }
        #endregion
    }
}
