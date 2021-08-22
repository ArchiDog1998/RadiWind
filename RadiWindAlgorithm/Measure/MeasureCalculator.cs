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
using Rhino.Input;
using Rhino.Geometry.Intersect;

namespace RadiWindAlgorithm.Measure
{
    public static class MeasureCalculator
    {
        private static void ListCalculate<T>(List<T> calculateObjects, Action<T, T> action, bool loop)
        {
            int count = calculateObjects.Count;
            int loopCount = loop ? count : count - 1;
            for (int i = 0; i < loopCount; i++)
            {
                action(calculateObjects[i], calculateObjects[(i + 1) % count]);
            }
        }

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
            double result;
            //StringParserSettings setting = new StringParserSettings();

            //if (StringParser.ParseNumber(number, 100, setting, ref setting, out result)!=0)
            //    throw new ArgumentOutOfRangeException(nameof(number), nameof(number) + "must be in a formular of number!");
            if (!double.TryParse(number, out result))
                throw new Exception();

            if (decimals >= 0)
            {
                return result.ToString("F" + decimals);
            }
            else
            {
                double t = (double)Math.Pow(10, Math.Abs(decimals));
                double multi = result / t;
                return (double.Parse(multi.ToString("F0")) * t).ToString();
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
        /// 
        /// </summary>
        /// <param name="points"></param>
        /// <param name="decimals"></param>
        /// <param name="loop"></param>
        /// <param name="plane"></param>
        /// <param name="displayLines"></param>
        /// <returns></returns>
        [Pythonable]
        public static List<string> HDistance(List<Point3d> points, int decimals, bool loop,Plane plane, out List<Line> displayLines)
        {
            List<string> distances = new List<string>();
            List<Line> displayLinesRelay = new List<Line>();

            ListCalculate(points, (pt1, pt2) =>
            {
                Line line;
                distances.Add(HDistance(pt1, pt2, plane, decimals, out line));
                displayLinesRelay.Add(line);
            }, loop);
            displayLines = displayLinesRelay;

            return distances;
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

        #region PPDistance
        /// <summary>
        /// Get the distance between plane and point
        /// </summary>
        /// <param name="point"></param>
        /// <param name="plane"></param>
        /// <param name="decimals">decimals count.</param>
        /// <param name="displayLine">a line to display</param>
        /// <returns>distance</returns>
        [Pythonable]
        public static string PPDistance(Point3d point, Plane plane, int decimals, out Line displayLine)
        {
            double distance = PPDistance(point, plane, out displayLine);
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
        public static double PPDistance(Point3d point, Plane plane, out Line displayLine)
        {
            displayLine = new Line(plane.ClosestPoint(point), point);
            return plane.DistanceTo(point);
        }
        #endregion

        #region Distance

        /// <summary>
        /// 
        /// </summary>
        /// <param name="points"></param>
        /// <param name="decimals"></param>
        /// <param name="loop"></param>
        /// <param name="displayLines"></param>
        /// <returns></returns>
        [Pythonable]
        public static List<string> Distance(List<Point3d> points, int decimals, bool loop, out List<Line> displayLines)
        {
            List<string> distances = new List<string>();
            List<Line> displayLinesRelay = new List<Line>();

            ListCalculate(points, (pt1, pt2) =>
            {
                 Line line;
                 distances.Add(Distance(pt1, pt2, decimals, out line));
                 displayLinesRelay.Add(line);
            }, loop);
            displayLines = displayLinesRelay;

            return distances;
        }

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

        //This method doesn't have a test.
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

        //I don't think such easy function needs a test.
        #region GetLength

        /// <summary>
        /// Get curve's length and end points.
        /// </summary>
        /// <param name="curve"></param>
        /// <param name="decimals">decimals count.</param>
        /// <param name="startPt"></param>
        /// <param name="endPt"></param>
        /// <returns>length.</returns>
        [Pythonable]
        public static string GetLength(Curve curve, int decimals, out Point3d startPt, out Point3d endPt)
        {
            double distance = GetLength(curve, out startPt, out endPt);
            return NumberDecimal(distance.ToString(), decimals);
        }

        /// <summary>
        /// Get curve's length and end points.
        /// </summary>
        /// <param name="curve"></param>
        /// <param name="startPt"></param>
        /// <param name="endPt"></param>
        /// <returns>length.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static double GetLength(Curve curve, out Point3d startPt, out Point3d endPt)
        {
            startPt = curve.PointAtStart;
            endPt = curve.PointAtEnd;
            return curve.GetLength();
        }
        #endregion

        //This method doesn't have a test.
        #region PCDistance

        /// <summary>
        /// Get the distance between curve and point.
        /// </summary>
        /// <param name="curve"></param>
        /// <param name="point"></param>
        /// <param name="decimals">decimals count.</param>
        /// <param name="displayLine">a line to display</param>
        /// <returns>distance</returns>
        [Pythonable]
        public static string PCDistance(Curve curve, Point3d point, int decimals, out Line displayLine)
        {
            double distance = PCDistance(curve, point, out displayLine);
            return NumberDecimal(distance.ToString(), decimals);
        }

        /// <summary>
        /// Get the distance between curve and point.
        /// </summary>
        /// <param name="curve"></param>
        /// <param name="point"></param>
        /// <param name="displayLine">a line to display</param>
        /// <returns>distance</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static double PCDistance(Curve curve, Point3d point, out Line displayLine)
        {
            //Get the closest point.
            double t = 0;
            curve.ClosestPoint(point, out t);
            Point3d closetPt = curve.PointAt(t);

            return Distance(point, closetPt, out displayLine);
        }
        #endregion

        //This method doesn't have a test.
        #region BrepLength
        /// <summary>
        /// Get the brep's longest edge's length and its params.
        /// </summary>
        /// <param name="brep"></param>
        /// <param name="usebox">whether use boundingbox to find longest edge.</param>
        /// <param name="decimals">decimals count.</param>
        /// <param name="longestCurve">longest edge itself.</param>
        /// <param name="boundingBox">brep's bounding box.</param>
        /// <param name="plane">edge's plane.</param>
        /// <returns>longest edge's length.</returns>
        [Pythonable]
        public static string BrepLength(Brep brep, bool usebox, int decimals, out Curve longestCurve, out Box boundingBox, out Plane plane)
        {
            double distance = BrepLength(brep, usebox, out longestCurve, out boundingBox, out plane);
            return NumberDecimal(distance.ToString(), decimals);
        }

        /// <summary>
        /// Get the brep's longest edge's length and its params.
        /// </summary>
        /// <param name="brep"></param>
        /// <param name="usebox">whether use boundingbox to find longest edge.</param>
        /// <param name="longestCurve">longest edge itself.</param>
        /// <param name="boundingBox">brep's bounding box.</param>
        /// <param name="plane">edge's plane.</param>
        /// <returns>longest edge's length.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static double BrepLength(Brep brep, bool usebox, out Curve longestCurve, out Box boundingBox, out Plane plane)
        {
            longestCurve = BrepLongestEdge(brep);

            //get base plane.
            if (!longestCurve.FrameAt(0, out plane))
                throw new Exception(nameof(longestCurve) + "failed to get frame.");
            plane = new Plane(plane.Origin, plane.YAxis, plane.ZAxis);

            //get boundingbox
            Transform transform = Transform.ChangeBasis(Plane.WorldXY, plane);
            BoundingBox box = brep.GetBoundingBox(transform);
            boundingBox = new Box(plane, box);

            if (usebox)
            {
                longestCurve = BrepLongestEdge(boundingBox.ToBrep());
            }

            return longestCurve.GetLength();
        }

        /// <summary>
        /// Get brep's longest edge and reparemeterize it.
        /// </summary>
        /// <param name="brep">brep to calculate.</param>
        /// <returns>longest edge</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static Curve BrepLongestEdge(Brep brep)
        {
            //get edges and sort it.
            List<Curve> curves = brep.DuplicateEdgeCurves().ToList();
            curves.Sort((x, y) => y.GetLength().CompareTo(x.GetLength()));

            //get longest edge and reparameterize it.
            Curve result = curves[0];
            result.Domain = new Interval(0, 1);

            //return.
            return result;
        }
        #endregion

        //This method doesn't have a test.
        #region PBDistance
        /// <summary>
        /// Get distance between brep and point.
        /// </summary>
        /// <param name="point"></param>
        /// <param name="pts"></param>
        /// <param name="brep"></param>
        /// <param name="decimals">decimals count.</param>
        /// <param name="displayLine"></param>
        /// <param name="closestPt"></param>
        /// <returns></returns>
        [Pythonable]
        public static string PBDistance(Point3d point, Point3d[] pts, ref Brep brep, int decimals,
            out Line displayLine, out Point3d closestPt)
        {
            double distance = PBDistance(point, pts, ref brep, out displayLine, out closestPt);
            return NumberDecimal(distance.ToString(), decimals);
        }


        /// <summary>
        /// Get distance between brep and point.
        /// </summary>
        /// <param name="point"></param>
        /// <param name="pts">a lot of test points.</param>
        /// <param name="brep"></param>
        /// <param name="displayLine"></param>
        /// <param name="closestPt"></param>
        /// <returns></returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static double PBDistance(Point3d point, Point3d[] pts, ref Brep brep, out Line displayLine, out Point3d closestPt)
        {
            if (pts.Length > 0)
            {
                Point3d averagePoint = AveragePoints(pts);

                //find the face who is closest to the averagePoint.
                List<Brep> breps = brep.Faces.Select((face) => face.DuplicateFace(false)).ToList();
                breps.Sort((x, y) => x.ClosestPoint(averagePoint).DistanceTo(averagePoint).CompareTo(
                    y.ClosestPoint(averagePoint).DistanceTo(averagePoint)));
                brep = breps[0];
            }
            return PBDistance(point, brep, out displayLine, out closestPt);
        }


        /// <summary>
        /// Get distance between brep and point.
        /// </summary>
        /// <param name="point"></param>
        /// <param name="brep"></param>
        /// <param name="displayLine"></param>
        /// <param name="closestPt"></param>
        /// <returns></returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static double PBDistance(Point3d point, Brep brep, out Line displayLine, out Point3d closestPt)
        {
            closestPt = brep.ClosestPoint(point);
            return Distance(point, closestPt, out displayLine);
        }

        /// <summary>
        /// Get average point.
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static Point3d AveragePoints(Point3d[] points)
        {
            Point3d result = new Point3d();
            foreach (Point3d point in points)
            {
                result += point;
            }
            return result / points.Length;
        }
        #endregion

        //This method doesn't have a test.
        #region CCAngle

        /// <summary>
        /// Get Curves' angle.
        /// </summary>
        /// <param name="curve1"></param>
        /// <param name="curve2"></param>
        /// <param name="decimals">decimals count.</param>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <param name="vec1"></param>
        /// <param name="vec2"></param>
        /// <returns></returns>
        [Pythonable]
        public static string CCAngle(Curve curve1, Curve curve2, int decimals, out Point3d point1, out Point3d point2, out Vector3d vec1, out Vector3d vec2)
        {
            double angle = CCAngle(curve1, curve2, out point1, out point2, out vec1, out vec2);
            return NumberDecimal(angle.ToString(), decimals);
        }

        /// <summary>
        /// Get Curves' angle.
        /// </summary>
        /// <param name="curve1"></param>
        /// <param name="curve2"></param>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <param name="vec1"></param>
        /// <param name="vec2"></param>
        /// <returns></returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static double CCAngle(Curve curve1, Curve curve2, out Point3d point1, out Point3d point2, out Vector3d vec1, out Vector3d vec2)
        {
            curve1.ClosestPoints(curve2, out point1, out point2);

            vec1 = curve1.GetVecOnCurve(point1);
            vec2 = curve2.GetVecOnCurve(point2);

            //Get angle in degree.
            double angle = Math.Acos(Math.Max(Math.Min(vec1 * vec2, 1.0), -1.0));
            return Rhino.RhinoMath.ToDegrees(angle);
        }

        /// <summary>
        /// Get vector on point.
        /// </summary>
        /// <param name="curve"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        private static Vector3d GetVecOnCurve(this Curve curve, Point3d point)
        {
            //Check for reverse.
            if (curve.PointAtStart.DistanceTo(point) > curve.PointAtEnd.DistanceTo(point)) curve.Reverse();

            //Get t
            double t;
            curve.ClosestPoint(point, out t);

            //Get vec and unitize it.
            Vector3d result = curve.DerivativeAt(t, 1, CurveEvaluationSide.Below)[1];
            result.Unitize();

            return result;
        }
        #endregion

        //This method doesn't have a test.
        #region SSAngle
        /// <summary>
        /// get the surfaces' angle.
        /// </summary>
        /// <param name="srf1"></param>
        /// <param name="srf2"></param>
        /// <param name="decimals">decimals count.</param>
        /// <param name="showPlane"></param>
        /// <param name="plane1"></param>
        /// <param name="plane2"></param>
        /// <returns></returns>
        [Pythonable]
        public static string SSAngle(Surface srf1, Surface srf2, int decimals, out Plane showPlane, out Plane plane1, out Plane plane2)
        {
            double angle = SSAngle(srf1, srf2, out showPlane, out plane1, out plane2);
            return NumberDecimal(angle.ToString(), decimals);
        }

        /// <summary>
        /// get the surfaces' angle.
        /// </summary>
        /// <param name="srf1"></param>
        /// <param name="srf2"></param>
        /// <param name="showPlane"></param>
        /// <param name="plane1"></param>
        /// <param name="plane2"></param>
        /// <returns></returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static double SSAngle(Surface srf1, Surface srf2, out Plane showPlane, out Plane plane1, out Plane plane2)
        {
            //Get surface's center infomation.
            Plane pl1 = srf1.GetBrepClosestPt();
            Plane pl2 = srf2.GetBrepClosestPt();

            double angleInDegree = PPAngle(ref pl1, ref pl2, out showPlane);
            plane1 = pl1;
            plane2 = pl2;
            return angleInDegree;
        }

        /// <summary>
        /// calculate the planes' angle.
        /// </summary>
        /// <param name="pl1"></param>
        /// <param name="pl2"></param>
        /// <param name="point"></param>
        /// <param name="showPlane"></param>
        /// <returns></returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static double PPAngle(ref Plane pl1, ref Plane pl2, out Plane showPlane)
        {
            //get the show Plane.
            Line interSectLine;
            Intersection.PlanePlane(pl1, pl2, out interSectLine);
            Point3d point = AveragePoints(new Point3d[] { pl1.Origin, pl2.Origin });
            showPlane = new Plane(interSectLine.ClosestPoint(point, true), interSectLine.Direction);

            //update the Plane.
            pl1 = new Plane(showPlane.ClosestPoint(pl1.Origin), pl1.XAxis, pl1.YAxis);
            pl2 = new Plane(showPlane.ClosestPoint(pl2.Origin), pl2.XAxis, pl2.YAxis);

            //calculate the angle
            double angle = Vector3d.VectorAngle(pl1.ZAxis, pl2.ZAxis);
            return Rhino.RhinoMath.ToDegrees(angle);
        }

        /// <summary>
        /// Get CenterPoint's location, direction, plane.
        /// </summary>
        /// <param name="srf"></param>
        /// <param name="vecMid"></param>
        /// <param name="plMid"></param>
        /// <returns></returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static Plane GetBrepClosestPt(this Surface srf)
        {
            //get centerPoint
            Point3d centerPoint = AveragePoints(srf.ToBrep().DuplicateVertices());

            //Get u v.
            double u, v;
            srf.ClosestPoint(centerPoint, out u, out v);

            //Get plane.
            Point3d pt;
            Vector3d[] der;
            srf.Evaluate(u, v, 1, out pt, out der);
            return new Plane(pt, der[0], der[1]);
        }
        #endregion

        //This method doesn't have a test.
        #region BrepArea
        public static Dictionary<int, UnitSet> Unit { get; } = new Dictionary<int, UnitSet>()
        {
            {0, new UnitSet( "平方毫米", 1000) },
            {1, new UnitSet( "平方米", 1) },
        };

        public struct UnitSet
        {
            public string Name { get; }
            public double Data { get; }
            public UnitSet(string name, double data)
            {
                this.Name = name;
                this.Data = data;
            }
        }

        /// <summary>
        /// Get the brep's area.
        /// </summary>
        /// <param name="brep"></param>
        /// <param name="decimals">decimals count.</param>
        /// <returns>area</returns>
        [Pythonable]
        public static string BrepArea(Brep brep, int decimals, int unit)
        {
            if (!Unit.ContainsKey(unit)) throw new ArgumentOutOfRangeException(nameof(unit) + "is out of range!");

            double mult = Unit[unit].Data;
            switch (Rhino.RhinoDoc.ActiveDoc.ModelUnitSystem)
            {
                case Rhino.UnitSystem.Millimeters:
                    mult /= 1000;
                    break;
                case Rhino.UnitSystem.Centimeters:
                    mult /= 100;
                    break;
                case Rhino.UnitSystem.Meters:
                    break;
                default:
                    break;
            }
            return NumberDecimal((brep.GetArea() * Math.Pow(mult, 2)).ToString(), decimals);
        }
        #endregion
    }
}
