/*  Copyright 2021 RadiRhino-秋水, 笑里追风. All Rights Reserved.

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

            if (decimals >= 0)
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
            if (plane == null)
            {
                plane = new Plane(point1, Vector3d.ZAxis);
            }

            Point3d pointA = plane.ClosestPoint(point1);
            Point3d pointB = plane.ClosestPoint(poitn2);

            return Distance(pointA, pointB, out displayLine);
        }
        #endregion

        #region PVDistance
        /// <summary>
        /// Get the distance between plane and point
        /// </summary>
        /// <param name="point"></param>
        /// <param name="plane"></param>
        /// <param name="decimals">decimals count.</param>
        /// <param name="displayLine">a line to display</param>
        /// <returns>distance</returns>
        [Pythonable]
        public static string PVDistance(Point3d point, Plane plane, int decimals, out Line displayLine)
        {
            double distance = PVDistance(point, plane, out displayLine);
            return NumberDecimal(distance.ToString(), decimals);
        }

        /// <summary>
        /// Get the distance between plane and point
        /// </summary>
        /// <param name="point"></param>
        /// <param name="plane"></param>
        /// <param name="displayLine">a line to display</param>
        /// <returns>distance</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static double PVDistance(Point3d point, Plane plane, out Line displayLine)
        {
            displayLine = new Line(plane.ClosestPoint(point), point);
            return plane.DistanceTo(point);
        }
        #endregion

        #region Distance

        /// <summary>
        /// Get two points' distance.
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <param name="decimals">decimals count.</param>
        /// <param name="displayLine">a line to display</param>
        /// <returns>distance</returns>
        [Pythonable]
        public static string Distance(Point3d point1, Point3d point2, int decimals, out Line displayLine)
        {
            double distance = Distance(point1, point2, out displayLine);
            return NumberDecimal(distance.ToString(), decimals);
        }

        /// <summary>
        /// Get two points' distance.
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <param name="displayLine">a line to display</param>
        /// <returns>distance</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static double Distance(Point3d point1, Point3d point2, out Line displayLine)
        {
            displayLine = new Line(point1, point2);
            return point1.DistanceTo(point2);
        }
        #endregion

        #region CCDistance
        /// <summary>
        /// Get two curves' distance.
        /// </summary>
        /// <param name="curve1"></param>
        /// <param name="curve2"></param>
        /// <param name="decimals">decimals count.</param>
        /// <param name="displayLine">a line to display</param>
        /// <returns>distance</returns>
        [Pythonable]
        public static string CCDistance(Curve curve1, Curve curve2, int decimals, out Line displayLine)
        {
            double distance = CCDistance(curve1, curve2, out displayLine);
            return NumberDecimal(displayLine.ToString(), decimals);
        }

        /// <summary>
        /// Get two curves' distance.
        /// </summary>
        /// <param name="curve1"></param>
        /// <param name="curve2"></param>
        /// <param name="displayLine">a line to display</param>
        /// <returns>distance</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static double CCDistance(Curve curve1, Curve curve2, out Line displayLine)
        {
            Point3d point1 = new Point3d();
            Point3d point2 = new Point3d();
            curve1.ClosestPoints(curve2, out point1, out point2);

            return Distance(point1, point2, out displayLine);
        }
        #endregion
    }
}
