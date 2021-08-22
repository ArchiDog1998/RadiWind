/*  Copyright 2020 RadiRhino-秋水, 笑里追风. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using RadiWindAlgorithm.Sort;
using Grasshopper;
using Grasshopper.Kernel.Data;
using Rhino.Geometry;

namespace RadiWind.Tests
{
    [TestClass]
    public class SortCalculatorTests
    {
        public static List<Point3d> TestPts => new List<Point3d>()
        {
            new Point3d(41, -52, 0),
            new Point3d(64, -22, 0),
            new Point3d(49, 67, 0),
            new Point3d(14, -56, 0),
            new Point3d(-30, -3, 0),
        };

        [TestMethod]
        public void SortByCircleTest()
        {
            List<int> expectIndex = new List<int>() { 2, 4, 3, 0, 1, };
            List<int> actualIndex;
            SortCalculator.SortByCircle(TestPts, Plane.WorldXY, 0, out _, out _, out actualIndex);

            bool flag = TestsHelper.IsListEqual(expectIndex, actualIndex, (x, y) => x == y);

            Assert.IsTrue(flag);
        }

        [TestMethod]
        public void PointAverangeDistanceTest()
        {
            double expectDis = 60.9548318574;
            double acutalDis = SortCalculator.PointAverangeDistance(new Point3d(), TestPts);

            bool flag = Math.Abs(expectDis - acutalDis) <= 1E-9;

            Assert.IsTrue(flag);
        }

        [TestMethod]
        public void PointAverangeTest()
        {
            Point3d expectPt = new Point3d(27.6, -13.2, 0);
            Point3d acutalPt = SortCalculator.AverangePoint(TestPts);

            Assert.AreEqual(expectPt, acutalPt);
        }

        [TestMethod]
        public void SortPointInAxisWithToleranceTest()
        {
            List<List<int>> exceptedList = new List<List<int>>()
            {
                new List<int>(){3, 0},
                new List<int>(){1, 4},
                new List<int>(){2},
            };

            List<List<int>> actualList;
            SortCalculator.SortPointInAxisWithTolerance(TestPts, 0, Plane.WorldYZ, 20, out actualList, out _);

            bool flag = TestsHelper.IsDoubleListEqual(exceptedList, actualList, (x, y) => x == y);

            Assert.IsTrue(flag);

        }

        [TestMethod]
        public void SortPointInAxisTest()
        {
            List<int> exceptIndex = new List<int>() { 3, 0, 1, 4, 2 };

            List<int> actualIndex;
            SortCalculator.SortPointInAxis(TestPts, 0, Plane.WorldYZ, out actualIndex);

            bool flag = TestsHelper.IsListEqual(exceptIndex, actualIndex, (x, y) => x == y);

            Assert.IsTrue(flag);


        }

        [TestMethod]
        public void NearlestPointSortByIndexTest()
        {
            List<int> exceptIndex = new List<int>() { 0, 3, 1, 2, 4 };

            List<int> actualIndex;
            SortCalculator.NearlestPointSortByIndex(TestPts, 0, out actualIndex);

            bool flag = TestsHelper.IsListEqual(exceptIndex, actualIndex, (x, y) => x == y);

            Assert.IsTrue(flag);
        }

        [TestMethod]
        public void NumberTolerancePartitionSortTest()
        {
            DataTree<double> testDatas = new DataTree<double>();
            testDatas.AddRange(new List<double>()
            {
                1.0, 1.1, 1.9, 2.0, 1.2, 1.8
            }, new GH_Path(0));
            testDatas.AddRange(new List<double>()
            {
                2.0, 2.1, 2.2, 2.8, 2.9, 3.0
            }, new GH_Path(1));

            DataTree<double> exceptDataTree = new DataTree<double>();
            exceptDataTree.AddRange(new List<double>()
            {
                1.0, 1.1, 1.2
            }, new GH_Path(0, 0));
            exceptDataTree.AddRange(new List<double>()
            {
                1.8, 1.9, 2.0
            }, new GH_Path(0, 1));
            exceptDataTree.AddRange(new List<double>()
            {
                2.0, 2.1, 2.2
            }, new GH_Path(1, 0));
            exceptDataTree.AddRange(new List<double>()
            {
                2.8, 2.9, 3.0
            }, new GH_Path(1, 1));

            DataTree<double> actualDataTree = SortCalculator.NumberTolerancePartitionSort(testDatas, 0.2, out _);

            bool flag = TestsHelper.IsDataTreeEqual(exceptDataTree, actualDataTree, (x, y) => x == y);

            Assert.IsTrue(flag);
        }

        [TestMethod]
        public void PointCurveSortTest()
        {
            //set the exceptIndex
            List<List<int>> exceptIndex = new List<List<int>>()
            {
                new List<int>(),
                new List<int>(){ 2, 4, 3, 0, 1, },
            };

            List<List<int>> actualIndex = new List<List<int>>();
            SortCalculator.PointCurveSort(TestPts, new List<Curve>()
            {
                new Circle(1).ToNurbsCurve(), new Circle(2).ToNurbsCurve(),
            }, out actualIndex);

            bool flag = TestsHelper.IsDoubleListEqual(exceptIndex, actualIndex, (x, y) => x == y);
            Assert.IsTrue(flag);
        }

        [TestMethod]
        public void XYPartitionSortedByXTest()
        {
            List<List<int>> exceptIndex = new List<List<int>>()
            {
                new List<int>(){ 3 },
                new List<int>(){ 0 },
                new List<int>(){ 1 },
                new List<int>(){ 4 },
                new List<int>(){ 2 },
            };

            List<List<int>> actualIndex;
            var actualSortableItems = SortCalculator.XYPartitionSortedByX(TestPts, Plane.WorldYZ,1, 1, out actualIndex, out _);

            bool flag = TestsHelper.IsDoubleListEqual(exceptIndex, actualIndex, (x, y) => x == y);
            Assert.IsTrue(flag);
        }

        #region Converter Test
        [TestMethod]
        public void GetSortableItemsTest()
        {
            List<int> testValue = new List<int>() { 10, 30, 20 };
            List<SortableItem<int>> expectResult = new List<SortableItem<int>>()
            {
                new SortableItem<int>(0, 10),
                new SortableItem<int>(1, 30),
                new SortableItem<int>(2, 20),
            };

            List<SortableItem<int>> actualResult = SortCalculator.GetSortableItems(testValue);

            bool flag = TestsHelper.IsListEqual(expectResult, actualResult, (x, y) =>
            {
                return x.Index == y.Index && x.Value == y.Value;
            });

            Assert.IsTrue(flag);
        }

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

            bool flag = TestsHelper.IsDoubleListEqual(expectValue, actualValue, (x, y) => x == y) 
                && TestsHelper.IsDoubleListEqual(expectIndex, actualIndex, (x, y) => x == y);

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

            bool flag = TestsHelper.IsListEqual(expectValue, actualValue, (x, y) => x == y) 
                && TestsHelper.IsListEqual(expectIndex, actualIndex, (x, y) => x == y);

            Assert.IsTrue(flag);

        }
        #endregion
        #endregion

    }
}
