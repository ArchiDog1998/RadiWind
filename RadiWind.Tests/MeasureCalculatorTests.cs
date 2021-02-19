/*  Copyright 2021 RadiRhino-秋水, 笑里追风. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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
    }
}
