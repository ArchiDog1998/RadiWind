/*  Copyright 2021 RadiRhino-秋水, 笑里追风. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Rhino.Geometry;
using RadiWindAlgorithm.Measure;

namespace RadiWind.Tests
{
    [TestClass]
    public class MeasureCalculatorTests
    {
        [TestMethod]
        public void NumberDecimalTest1()
        {
            string input = "12.344";
            string except = "12.3";

            Assert.AreEqual(except, MeasureCalculator.NumberDecimal(input, 1));
        }

        [TestMethod]
        public void NumberDecimalTest2()
        {
            string input = "1234.4";
            string except = "1200";

            Assert.AreEqual(except, MeasureCalculator.NumberDecimal(input, -2));
        }

        [TestMethod]
        public void HDistanceTest()
        {
            Point3d point1 = new Point3d(4, 5, 6);
            Point3d point2 = new Point3d(1, 1, 1);
            Plane testPlane = Plane.WorldXY;

            double actualDistance = MeasureCalculator.HDistance(point1, point2, testPlane, out _);
            double exceptDistance = 5;

            Assert.AreEqual(exceptDistance, actualDistance);
        }

        [TestMethod]
        public void PVDistanceTest()
        {
            Point3d point = new Point3d(4, 5, 6);
            Plane testPlane = Plane.WorldYZ;

            double actualDistance = MeasureCalculator.PVDistance(point, testPlane, out _);
            double exceptDistance = 4;

            Assert.AreEqual(exceptDistance, actualDistance);
        }
    }
}
