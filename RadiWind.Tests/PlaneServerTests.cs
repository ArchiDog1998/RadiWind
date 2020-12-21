/*  Copyright 2020 RadiRhino-秋水, 笑里追风. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Rhino.Geometry;
using RadiWindAlgorithm;

namespace RadiWind.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class PlaneServerTests
    {

        [TestMethod]
        public void PlaneCoordinateTest()
        {
            Point3d testPt = new Point3d(10, 20, 30);

            Point3d resultPt = PlaneServer.PlaneCoordinate(Plane.WorldXY, testPt);

            Assert.AreEqual(testPt, resultPt);
        }
    }
}
