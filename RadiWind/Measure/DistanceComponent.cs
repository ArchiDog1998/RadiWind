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
    public class DistanceComponent : BaseMeasureComponent
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
          : base("DistanceComponent", "Distance", "Description")
        {
        }

        #region Calculate
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Points", "P", "Points", GH_ParamAccess.list);
            pManager.AddBooleanParameter("Loop", "L", "Loop", GH_ParamAccess.item, false);
            base.RegisterInputParams(pManager);

            this.Message = "两点距离";

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddLineParameter("Display Line", "L", "Display Line", GH_ParamAccess.list);
            pManager.AddTextParameter("Distance", "D", "Distance", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Point3d> pts = new List<Point3d>();
            bool loop = false;

            DA.GetDataList(0, pts);
            DA.GetData(1, ref loop);

            List<Line> displayLine = new List<Line>();
            DA.SetDataList(1, MeasureCalculator.Distance(pts, Decimal, loop, out displayLine));
            DA.SetDataList(0, displayLine);
        }
        #endregion
    }
}