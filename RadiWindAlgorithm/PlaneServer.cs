﻿/*  Copyright 2020 RadiRhino-秋水, 笑里追风. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadiWindAlgorithm
{
    public static class PlaneServer
    {
        public static List<Point3d> PlaneCoordinate(Plane basePlane, List<Point3d> ptsInWorld)
        {
            List<Point3d> outPts = new List<Point3d>();
            foreach (var pt in ptsInWorld)
            {
                outPts.Add(PlaneCoordinate(basePlane, pt));
            }
            return outPts;
        }

        public static Point3d PlaneCoordinate(Plane basePlane, Point3d ptInWorld)
        {
            double x = 0;
            double y = 0;
            basePlane.ClosestParameter(ptInWorld, out x, out y);

            Point3d closestPt = basePlane.ClosestPoint(ptInWorld);
            double z = closestPt.DistanceTo(ptInWorld);

            return new Point3d(x, y, z);
        }
    }
}
