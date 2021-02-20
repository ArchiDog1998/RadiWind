﻿/*  Copyright 2021 RadiRhino-秋水, 笑里追风. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;

namespace RadiWindAlgorithm.Measure
{
    public static class MeasureCalculator
    {
        #region NumberDecimal
        /// <summary>
        /// Convert a text into the right format.
        /// </summary>
        /// <param name="number">number in text.</param>
        /// <param name="decimals">decimals count.</param>
        /// <returns>fomatted text.</returns>
        [Pythonable]
        public static string NumberDecimal(string number, int decimals)
        {
            decimal result;
            if (!decimal.TryParse(number, out result))
                throw new ArgumentOutOfRangeException(nameof(number), nameof(number) + "must be in a formular of number!");
            
            if(decimals >= 0)
            {
                return result.ToString("F" + decimals);
            }
            else
            {
                decimal t = (decimal)Math.Pow(10, Math.Abs(decimals));
                decimal multi = result / t;
                return (decimal.Parse(multi.ToString("F0")) * t).ToString();
            }
        }
        #endregion

        #region HDistance
        /// <summary>
        /// Get the horizonal distance between two points based on one plane.
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="poitn2"></param>
        /// <param name="plane"></param>
        /// <param name="decimals">decimals count.</param>
        /// <param name="displayLine">a line to display</param>
        /// <returns>distance</returns>
        [Pythonable]
        public static string HDistance(Point3d point1, Point3d poitn2, Plane plane, int decimals, out Line displayLine)
        {
            double distance = HDistance(point1, poitn2, plane, out displayLine);
            return NumberDecimal(distance.ToString(), decimals);
        }

        /// <summary>
        /// Get the horizonal distance between two points based on one plane.
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="poitn2"></param>
        /// <param name="plane"></param>
        /// <param name="displayLine">a line to display</param>
        /// <returns>distance</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static double HDistance(Point3d point1, Point3d poitn2, Plane plane, out Line displayLine)
        {
            if(plane == null)
            {
                plane = new Plane(point1, Vector3d.ZAxis);
            }

            Point3d pointA = plane.ClosestPoint(point1);
            Point3d pointB = plane.ClosestPoint(poitn2);


            displayLine = new Line(pointA, pointB);

            return pointA.DistanceTo(pointB);
        }
        #endregion
    }
}