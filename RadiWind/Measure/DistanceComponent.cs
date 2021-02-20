/*  Copyright 2021 RadiRhino-秋水. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;
using RadiWindAlgorithm.Measure;

namespace RadiWind.Measure
{
    public class DistanceComponent : GH_Component
    {
        #region Values
        #region Basic Component info

        public override GH_Exposure Exposure => GH_Exposure.secondary;

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override Bitmap Icon => Properties.Resources.Zp_测量_03两点距离;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("e0d17479-7195-4eee-976c-05d63f3ed9d6");

        #endregion
        #endregion

        /// <summary>
        /// Initializes a new instance of the DistanceComponent class.
        /// </summary>
        public DistanceComponent()
          : base("DistanceComponent", "Distance",
              "Description",
              "RadiWind", "Measure")
        {
        }

        #region Calculate
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Point A", "A", "Point A", GH_ParamAccess.item);
            pManager.AddPointParameter("Point B", "B", "Point B", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Decimals", "D", "Decimals", GH_ParamAccess.item, 0);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddLineParameter("Display Line", "L", "Display Line", GH_ParamAccess.item);
            pManager.AddTextParameter("Distance", "D", "Distance", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Point3d point1 = Point3d.Origin;
            Point3d point2 = Point3d.Origin;
            int decimals = 0;

            DA.GetData(0, ref point1);
            DA.GetData(1, ref point2);
            DA.GetData(2, ref decimals);

            Line displayLine = new Line();
            DA.SetData(1, MeasureCalculator.Distance(point1, point2, out displayLine));
            DA.SetData(0, displayLine);
        }
        #endregion
    }
}