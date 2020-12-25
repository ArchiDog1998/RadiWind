/*  Copyright 2020 RadiRhino-秋水, 笑里追风. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using Grasshopper;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace RadiWindAlgorithm.Sort
{
    public static class SortCalculator
    {
        #region PointCurveSort

        /// <summary>
        /// Sort point along curve and group it with the closest curve.
        /// </summary>
        /// <param name="points">points waited to sort</param>
        /// <param name="curves">all curves</param>
        /// <param name="indexes">indexes</param>
        /// <returns>sorted points</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static List<List<Point3d>> PointCurveSort(List<Point3d> points, List<Curve> curves, out List<List<int>> indexes)
        {
            List<SortableItem<Point3d>> sortableItems = GetSortableItems(points);
            List<int> curveIndexes = GetClosestCurveIndex(points, curves);

            //Participate them into different list.
            List<List<SortableItem<Point3d>>> ParticipateePoints = new List<List<SortableItem<Point3d>>>();
            for (int i = 0; i < curves.Count; i++)
            {
                ParticipateePoints.Add(new List<SortableItem<Point3d>>());
            }

            for (int j = 0; j < curveIndexes.Count; j++)
            {
                ParticipateePoints[curveIndexes[j]].Add(sortableItems[j]);
            }

            //Sort it.
            List<List<SortableItem<Point3d>>> sortedPoints = new List<List<SortableItem<Point3d>>>();
            for (int k = 0; k < curves.Count; k++)
            {
                SortPtAlongCurve(ParticipateePoints[k], curves[k]);
            }

            return DispatchIt(sortedPoints, out indexes);
        }

        /// <summary>
        /// Get the Closest Curve's Index.
        /// </summary>
        /// <param name="points">the point list</param>
        /// <param name="curves">all curves</param>
        /// <returns>the indexes of curves</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static List<int> GetClosestCurveIndex(List<Point3d> points, List<Curve> curves)
        {
            List<int> indexes = new List<int>();
            foreach (Point3d point in points)
            {
                indexes.Add(GetClosestCurveIndex(point, curves));
            }
            return indexes;
        }

        /// <summary>
        /// Get the Closest Curve's Index.
        /// </summary>
        /// <param name="point">the point</param>
        /// <param name="curves">all curves</param>
        /// <returns>the index of curves.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static int GetClosestCurveIndex(Point3d point, List<Curve> curves)
        {
            int index = 0;
            double minDistance;
            if (!curves[0].ClosestPoint(point, out minDistance))
                throw new Exception("ClosestPoint failed to calculate!");

            for (int i = 1; i < curves.Count; i++)
            {
                double distance;
                if (!curves[i].ClosestPoint(point, out distance))
                    throw new Exception("ClosestPoint failed to calculate!");
                if (distance < minDistance)
                    minDistance = distance;
            }
            return index;
        }

        #endregion

        #region PointsPartitionPlaneSort

        /// <summary>
        /// Partition the points in X, Y two direcitons and then Sort them with Z Direction.
        /// </summary>
        /// <param name="inputPoints">input pointDataTree</param>
        /// <param name="basePlane"></param>
        /// <param name="xTol">x tolerance</param>
        /// <param name="yTol">y tolerance</param>
        /// <param name="indexes">sorted indexes</param>
        /// <returns>sorted points DataTree</returns>
        [Pythonable]
        public static DataTree<Point3d> XYPartitionSortedByX(DataTree<Point3d> inputPoints, Plane basePlane, double xTol, double yTol, out DataTree<int> indexes, out DataTree<Rectangle3d> showRect)
        {
            DataTree<Point3d> outTree = new DataTree<Point3d>();
            indexes = new DataTree<int>();
            showRect = new DataTree<Rectangle3d>();
            for (int i = 0; i < inputPoints.BranchCount; i++)
            {
                List<List<int>> resultindexes;
                List<List<Rectangle3d>> resultRect;
                List<List<Point3d>> resultTree = XYPartitionSortedByX(inputPoints.Branches[i], basePlane, xTol, yTol, out resultindexes, out resultRect);
                outTree.SetDataIntoDataTree(resultTree, i);
                showRect.SetDataIntoDataTree(resultRect, i);
                indexes.SetDataIntoDataTree(resultindexes, i);
            }
            return outTree;
        }

        /// <summary>
        /// Partition the points in X, Y two direcitons and then Sort them with Z Direction.
        /// </summary>
        /// <param name="inputPoints">input pointList</param>
        /// <param name="basePlane"></param>
        /// <param name="xTol">x tolerance</param>
        /// <param name="yTol">y tolerance</param>
        /// <param name="indexes">sorted indexes</param>
        /// <returns>sorted points</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static List<List<Point3d>> XYPartitionSortedByX(List<Point3d> inputPoints, Plane basePlane, double xTol, double yTol, out List<List<int>> indexes, out List<List<Rectangle3d>> showRect)
        {
            List<Point3d> relativePts = PlaneServer.PlaneCoordinate(basePlane, inputPoints);
            List<List<SortableItem<Point3d>>> result = XYPartitionSortedByX(relativePts, xTol, yTol);
            //Maybe some bugs in this transform.
            List<List<Point3d>> resultPoints = DispatchIt(result, out indexes, (x) => basePlane.PointAt(x.X, x.Y, x.Z));

            //Get the ShowRects.
            showRect = new List<List<Rectangle3d>>();
            foreach (List<Point3d> point3Ds in resultPoints)
            {
                List<Rectangle3d> relayRects = new List<Rectangle3d>();
                foreach (Point3d point in point3Ds)
                {
                    relayRects.Add(new Rectangle3d(new Plane(point, basePlane.XAxis, basePlane.YAxis),
                        new Interval(-xTol / 4, xTol / 4), new Interval(-yTol / 4, yTol / 4)));
                }
                showRect.Add(relayRects);
            }
            return resultPoints;
        }


        /// <summary>
        /// Partition the points in X, Y two direcitons and then Sort them with Z Direction.
        /// </summary>
        /// <param name="inputPoints">input pointList</param>
        /// <param name="xTol">x tolerance</param>
        /// <param name="yTol">y tolerance</param>
        /// <returns>Participated and sorted sortable Points.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static List<List<SortableItem<Point3d>>> XYPartitionSortedByX(List<Point3d> inputPoints, double xTol, double yTol)
        {
            //Get the Partition about x Direction
            List<List<SortableItem<Point3d>>> xPartitions = NumberTolerancePartitionSort(inputPoints, (x) => x.X, xTol);

            List<List<SortableItem<Point3d>>> result = new List<List<SortableItem<Point3d>>>();

            //Make every xPartition parted by y Dierction.
            foreach (List<SortableItem<Point3d>> xPartition in xPartitions)
            {
                foreach (List<SortableItem<Point3d>> partition in NumberTolerancePartitionSort<Point3d>(xPartition, (x) => x.Y, yTol))
                {
                    //Sort in X direction.
                    partition.Sort((x, y) => x.Value.X.CompareTo(y.Value.X));
                    result.Add(partition);
                }
            }
            return result;
        }


        #endregion

        #region SortByCircle

        /// <summary>
        /// Sort the points by Circle
        /// </summary>
        /// <param name="inputList">input Points</param>
        /// <param name="basePlane">base Plane</param>
        /// <param name="rotate">rotate in radius</param>
        /// <param name="showCircle">the circle that should show</param>
        /// <param name="showPlane">the plane that should show</param>
        /// <param name="indexes">sorted indexes.</param>
        /// <returns>sorted points.</returns>
        [Pythonable]
        public static List<Point3d> SortByCircle(List<Point3d> inputList, Plane basePlane, double rotate, out Line showLine, out Plane showPlane, out List<int> indexes)
        {
            if (rotate != 0)
                basePlane.Rotate(rotate, basePlane.ZAxis, basePlane.Origin);

            Point3d origin;
            Circle alignCir = GetFlagCurve(basePlane, inputList, out showLine, out origin);
            showPlane = new Plane(origin, basePlane.XAxis, basePlane.YAxis);

            List<SortableItem<Point3d>> result = SortPtAlongCurve(GetSortableItems(inputList), alignCir.ToNurbsCurve());

            return DispatchIt(result, out indexes);
        }

        /// <summary>
        /// Sort Points along Curve.
        /// </summary>
        /// <param name="inputSortablePts">input pointSortableList</param>
        /// <param name="alineCurve">aline Curve</param>
        /// <returns>Sorted pointSortableList</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static List<SortableItem<Point3d>> SortPtAlongCurve(List< SortableItem<Point3d>> inputSortablePts, Curve alineCurve)
        {
            inputSortablePts.Sort((x, y) =>
            {
                double tX;
                alineCurve.ClosestPoint(x.Value, out tX);
                double tY;
                alineCurve.ClosestPoint(y.Value, out tY);
                return tX.CompareTo(tY);
            });
            return inputSortablePts;
        }

        /// <summary>
        /// Get the right Aline Circle
        /// </summary>
        /// <param name="basePlane">reference plane</param>
        /// <param name="inputList">input pointList</param>
        /// <returns>the circle that should be along.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static Circle GetFlagCurve(Plane basePlane, List<Point3d> inputList, out Line showLine, out Point3d origin)
        {
            origin = AverangePoint(inputList);
            double radius = PointAverangeDistance(origin, inputList);
            Plane drawPlane = new Plane(origin, basePlane.XAxis, basePlane.YAxis);
            showLine = new Line(origin, drawPlane.XAxis, radius);
            return new Circle(drawPlane, radius);
        }

        /// <summary>
        /// Get the averange of poinstList's distance to the anchor.
        /// </summary>
        /// <param name="anchor">the base point</param>
        /// <param name="inputList">input pointList</param>
        /// <returns>averange Distance</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static double PointAverangeDistance(Point3d anchor, List<Point3d> inputList)
        {
            double wholeDistance = 0;
            foreach (Point3d pt in inputList)
            {
                wholeDistance += anchor.DistanceTo(pt);
            }
            return wholeDistance / inputList.Count;
        }

        /// <summary>
        /// Get pointsList's averange point
        /// </summary>
        /// <param name="inputList">input pointList</param>
        /// <returns>the averange point</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static Point3d AverangePoint (List<Point3d> inputList)
        {
            Point3d addedPt = new Point3d(0, 0, 0);
            foreach (Point3d pt in inputList)
            {
                addedPt += pt;
            }
            return addedPt / inputList.Count;
        }

        #endregion

        #region  SortPointInAxisWithTolerance
        /// <summary>
        /// Sort the point in one axis and group it with tolerance.
        /// </summary>
        /// <param name="inputPts">points that inputs. </param>
        /// <param name="axisType">which axis should input, acutally is the points' index</param>
        /// <param name="basePlane">basePlane to confirm</param>
        /// <param name="tolerance">tolerance</param>
        /// <param name="indexes">the participated indexs</param>
        /// <returns>particiapted points</returns>
        [Pythonable]
        public static List<List<Point3d>> SortPointInAxisWithTolerance(List<Point3d> inputPts, int axisType, Plane basePlane, double tolerance, out List<List<int>> indexes)
        {
            List<SortableItem<Point3d>> sortedItems = SortCalculator.SortPointInAxis(inputPts, axisType);
            List<List<SortableItem<Point3d>>> participatedItems = SortCalculator.NumberTolerancePartitionSort<Point3d>(sortedItems, (x) =>
            {
                return PlaneServer.PlaneCoordinate(basePlane, x)[axisType];
            }, tolerance);

            return SortCalculator.DispatchIt(participatedItems, out indexes);
        }
        #endregion

        #region  SortPointInAxis
        /// <summary>
        /// Sort point by one axis.
        /// </summary>
        /// <param name="points">points to input</param>
        /// <param name="axisType">an integer for type between x, y, z.</param>
        /// <param name="indexes">the sorted points' index</param>
        /// <returns>the sorted points.</returns>
        [Pythonable]
        public static List<Point3d> SortPointInAxis(List<Point3d> points, int axisType, Plane plane, out List<int> indexes)
        {
            List<Point3d> waitToSort = PlaneServer.PlaneCoordinate(plane, points);
            return SortPointInAxis(waitToSort, axisType, out indexes);
        }

        /// <summary>
        /// Sort point by one axis.
        /// </summary>
        /// <param name="points">points to input</param>
        /// <param name="axisType">an integer for type between x, y, z.</param>
        /// <param name="indexes">the sorted points' index</param>
        /// <returns>the sorted points.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static List<Point3d> SortPointInAxis(List<Point3d> points, int axisType, out List<int> indexes)
        {
            return DispatchIt(SortPointInAxis(points, axisType), out indexes);
        }

        /// <summary>
        /// Sort point by one axis.
        /// </summary>
        /// <param name="points">points to input</param>
        /// <param name="axisType">an integer for type between x, y, z.</param>
        /// <returns>the sorted sortableItems.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static List<SortableItem<Point3d>> SortPointInAxis(List<Point3d> points, int axisType)
        {
            if (axisType < 0 || axisType > 2)
                throw new ArgumentOutOfRangeException("type", "type must be in 0-2!");

            List<SortableItem<Point3d>> needToSorItems = GetSortableItems(points);
            needToSorItems.Sort((x, y) => x.Value[axisType].CompareTo(y.Value[axisType]));
            return needToSorItems;
        }

        #endregion

        #region NumberTolerancePartion
        /// <summary>
        /// Sort number and partition them with Tolerance.
        /// </summary>
        /// <param name="numberTree">the number Tree that inputs in.</param>
        /// <param name="tolerance">tolerance</param>
        /// <param name="indexes">the sorted numbers' indexes.</param>
        /// <returns>the sorted numbers.</returns>
        [Pythonable]
        public static DataTree<double> NumberTolerancePartitionSort(DataTree<double> numberTree, double tolerance, out DataTree<int> indexes)
        {
            DataTree<double> outTree = new DataTree<double>();
            indexes = new DataTree<int>();
            for (int i = 0; i < numberTree.BranchCount; i++)
            {
                List<List<int>> resultindexes;
                List<List<double>> resultTree = NumberTolerancePartitionSort(numberTree.Branches[i], tolerance, out resultindexes);
                outTree.SetDataIntoDataTree(resultTree, i);
                indexes.SetDataIntoDataTree(resultindexes, i);
            }
            return outTree;
        }

        /// <summary>
        /// Sort number and partition them with Tolerance.
        /// </summary>
        /// <param name="numberList">list of numbers to input.</param>
        /// <param name="tolerance">tolerance</param>
        /// <param name="indexes">the sorted numbers' indexes.</param>
        /// <returns>the sorted numbers.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static List<List<double>> NumberTolerancePartitionSort(List<double> numberList, double tolerance, out List<List<int>> indexes)
        {
            return NumberTolerancePartitionSort<double>(numberList, (x) => x, tolerance, out indexes);
        }

        /// <summary>
        /// Partion sort with tolerance in double.
        /// </summary>
        /// <typeparam name="T">ValueType</typeparam>
        /// <param name="values">ValueList</param>
        /// <param name="getDouble">how to know the key.</param>
        /// <param name="tolerance">tolerance in double</param>
        /// <param name="indexs">out in index.</param>
        /// <returns>sorted numbers.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static List<List<T>> NumberTolerancePartitionSort<T>(List<T> values, Func<T, double> getDouble, double tolerance, out List<List<int>> indexs)
        {
            List<List<SortableItem<T>>> sortableTree = NumberTolerancePartitionSort<T>(values, getDouble, tolerance);
            return DispatchIt<T>(sortableTree, out indexs);
        }

        /// <summary>
        /// Partion sort with tolerance in double.
        /// </summary>
        /// <typeparam name="T">ValueType</typeparam>
        /// <param name="values">ValueList</param>
        /// <param name="getDouble">how to know the key.</param>
        /// <param name="tolerance">tolerance in double</param>
        /// <returns>sorted numbers in sortableItem.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static List<List<SortableItem<T>>> NumberTolerancePartitionSort<T>(List<T> values, Func<T, double> getDouble, double tolerance)
        {
            List<SortableItem<T>> sortableItems = GetSortableItems<T>(values);
            return NumberTolerancePartitionSort<T>(sortableItems, getDouble, tolerance);
        }

        /// <summary>
        /// Partion sort with tolerance in double.
        /// </summary>
        /// <typeparam name="T">ValueType</typeparam>
        /// <param name="sortableItems"> sortableItems to input.</param>
        /// <param name="getDouble">how to know the key.</param>
        /// <param name="tolerance">tolerance in double</param>
        /// <returns>sorted numbers in sortableItem.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static List<List<SortableItem<T>>> NumberTolerancePartitionSort<T>(List<SortableItem<T>> sortableItems, Func<T, double> getDouble, double tolerance)
        {
            //Sort it
            sortableItems.Sort((x, y) =>
            {
                return getDouble.Invoke(x.Value).CompareTo(getDouble.Invoke(y.Value));
            });

            //Participate it
            List<List<SortableItem<T>>> sortableTree = new List<List<SortableItem<T>>>() { new List<SortableItem<T>>() { sortableItems[0] } };
            for (int i = 1; i < sortableItems.Count; i++)
            {
                //if the gap bigger than tolerance
                if (getDouble(sortableItems[i].Value) - getDouble(sortableItems[i - 1].Value) >= tolerance)
                {
                    List<SortableItem<T>> list = new List<SortableItem<T>>() { sortableItems[i] };
                    sortableTree.Add(list);
                }
                else
                {
                    sortableTree[sortableTree.Count - 1].Add(sortableItems[i]);
                }
            }
            return sortableTree;
        }
        #endregion

        #region NearlestPointSortByIndex
        /// <summary>
        /// Sort the nearlest point.the first point is selected by index.
        /// </summary>
        /// <param name="inputPoints">points list</param>
        /// <param name="index">first point's index.</param>
        /// <param name="indexes">sorted points' indexes.</param>
        /// <returns>sorted points</returns>
        [Pythonable]
        public static List<Point3d> NearlestPointSortByIndex(List<Point3d> inputPoints, int index, out List<int> indexes)
        {
            List<SortableItem<Point3d>> result = NearlestPointSortByIndex(GetSortableItems(inputPoints), index);
            return DispatchIt<Point3d>(result, out indexes);
        }

        /// <summary>
        /// Sort the nearlest point.the first point is selected by index.
        /// </summary>
        /// <param name="inpuSortableItems">sortable items' list</param>
        /// <param name="index">first point's index.</param>
        /// <returns>sorted sortable items.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal static List<SortableItem<Point3d>> NearlestPointSortByIndex(List<SortableItem<Point3d>> inpuSortableItems, int index)
        {
            List<SortableItem<Point3d>> outItems = new List<SortableItem<Point3d>>() { inpuSortableItems[index] };
            inpuSortableItems.RemoveAt(index);

            while (inpuSortableItems.Count > 0)
            {
                Point3d flagPt = outItems[outItems.Count - 1].Value;

                //Find the point that is nearlist to the flagPt.
                SortableItem<Point3d> nearlistItem = inpuSortableItems[0];
                double minDistance = flagPt.DistanceTo(nearlistItem.Value);
                for (int i = 1; i < inpuSortableItems.Count; i++)
                {
                    double distance = flagPt.DistanceTo(inpuSortableItems[i].Value);
                    if (distance < minDistance)
                    {
                        nearlistItem = inpuSortableItems[i];
                        minDistance = distance;
                    }
                }

                //Remove and Add.
                outItems.Add(nearlistItem);
                inpuSortableItems.Remove(nearlistItem);
            }
            return outItems;
        }
        #endregion

        #region Converter for SortableItem
        /// <summary>
        /// Get the SortableItems.
        /// </summary>
        /// <typeparam name="T">Value Type</typeparam>
        /// <param name="values">Value in list</param>
        /// <returns>sortableItems' list</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static List<SortableItem<T>> GetSortableItems<T>(List<T> values)
        {
            List<SortableItem<T>> outList = new List<SortableItem<T>>();
            for (int i = 0; i < values.Count; i++)
            {
                outList.Add(new SortableItem<T>(i, values[i]));
            }
            return outList;
        }

        /// <summary>
        /// Dispatch the list form sortableItem to value and index.
        /// </summary>
        /// <typeparam name="T">Value Type</typeparam>
        /// <param name="sortableTree">Value in double list.</param>
        /// <param name="indexes">out Index.</param>
        /// <returns>values' double list.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static List<List<T>> DispatchIt<T>(List<List<SortableItem<T>>> sortableTree, out List<List<int>> indexes, Func<T, T> converter = null)
        {
            converter = converter ?? ((x) => x);

            List<List<T>> outList = new List<List<T>>();
            indexes = new List<List<int>>();

            foreach (var sortableList in sortableTree)
            {
                List<T> valueList = new List<T>();
                List<int> indexList = new List<int>();

                foreach (var item in sortableList)
                {
                    valueList.Add(converter.Invoke(item.Value));
                    indexList.Add(item.Index);
                }

                outList.Add(valueList);
                indexes.Add(indexList);
            }

            return outList;
        }

        /// <summary>
        /// Dispatch the list form sortableItem to value and index.
        /// </summary>
        /// <typeparam name="T">Value type</typeparam>
        /// <param name="sortableList">input sortableItems' list</param>
        /// <param name="indexes">dispatched indexes.</param>
        /// <returns>dispatched values.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static List<T> DispatchIt<T>(List<SortableItem<T>> sortableList, out List<int> indexes, Func<T, T> converter = null)
        {

            converter = converter ?? ((x) => x);
            indexes = new List<int>();
            List<T> values = new List<T>();
            foreach (var item in sortableList)
            {
                values.Add(converter.Invoke(item.Value));
                indexes.Add(item.Index);
            }
            return values;
        }
        #endregion
    }
}
