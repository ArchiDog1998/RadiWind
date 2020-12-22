/*  Copyright 2020 RadiRhino-秋水, 笑里追风. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using RadiWindAlgorithm.Sort;

namespace RadiWind.Tests
{
    [TestClass]
    public class SortCalculatorTests
    {
        #region Test DipatchIt
        [TestMethod]
        public void DispatchItDoubleListTest()
        {
            #region Set Values
            List<List<SortableItem<int>>> testDoubleList = new List<List<SortableItem<int>>>()
            {
                new List<SortableItem<int>>()
                {
                    new SortableItem<int>(0, 10),
                    new SortableItem<int>(2, 30),
                    new SortableItem<int>(1, 20),
                },
                new List<SortableItem<int>>()
                {
                    new SortableItem<int>(10, 110),
                    new SortableItem<int>(12, 130),
                    new SortableItem<int>(11, 120),
                }
            };

            List<List<int>> expectValue = new List<List<int>>()
            {
                new List<int>(){10, 30, 20},
                new List<int>(){110, 130, 120}
            };
            List<List<int>> expectIndex = new List<List<int>>()
            {
                new List<int>(){0, 2, 1},
                new List<int>(){10, 12, 11},
            };

            List<List<int>> actualIndex;
            List<List<int>> actualValue = SortCalculator.DispatchIt(testDoubleList, out actualIndex);
            #endregion

            bool flag = IsDoubleListEqual(expectValue, actualValue) && IsDoubleListEqual(expectIndex, actualIndex);

            Assert.IsTrue(flag);

        }

        [TestMethod]
        public void DispatchItListTest()
        {
            #region Set Value
            List<SortableItem<int>> testList = new List<SortableItem<int>>()
            {
                new SortableItem<int>(0, 10),
                new SortableItem<int>(2, 30),
                new SortableItem<int>(1, 20),
            };
            List<int> expectValue = new List<int>() { 10, 30, 20 };
            List<int> expectIndex = new List<int>() { 0, 2, 1 };

            List<int> actualIndex;
            List<int> actualValue = SortCalculator.DispatchIt(testList, out actualIndex);
            #endregion

            bool flag = IsListEqual(expectValue, actualValue) && IsListEqual(expectIndex, actualIndex);

            Assert.IsTrue(flag);

        }
        #endregion

        #region Is List Equal
        /// <summary>
        /// Find out is List equal
        /// </summary>
        /// <param name="expectList">list a</param>
        /// <param name="actualList">list b</param>
        /// <returns>is equal</returns>
        private bool IsListEqual(List<int> expectList, List<int> actualList)
        {
            if (expectList.Count != actualList.Count)
                return false;

            for (int i = 0; i < expectList.Count; i++)
            {
                if (expectList[i] != actualList[i])
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
        private bool IsDoubleListEqual(List<List<int>> expectList, List<List<int>> actualList)
        {
            if (expectList.Count != actualList.Count)
                return false;

            for (int i = 0; i < expectList.Count; i++)
            {
                if (!IsListEqual( expectList[i], actualList[i]))
                    return false;
            }

            return true;
        }
        #endregion
    }
}
