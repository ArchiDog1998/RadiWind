/*  Copyright 2021 RadiRhino-秋水, 笑里追风. All Rights Reserved.

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
    public class HDistanceComponent : BaseMeasureComponent
    {
        #region Values
        #region Basic Component info

        public override GH_Exposure Exposure => GH_Exposure.secondary;

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override Bitmap Icon => Properties.Resources.Zp_测量_03两点水平距离;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("bd6b91d7-c1cc-446f-8238-800e3618f923");

        #endregion
        #endregion

        /// <summary>
        /// Initializes a new instance of the HDistanceComponent class.
        /// </summary>
        public HDistanceComponent()
          : base("HDistanceComponent", "HDistance", "Description")
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
            pManager.AddPlaneParameter("Plane", "Pl", "Plane", GH_ParamAccess.item);
            pManager[2].Optional = true;
            base.RegisterInputParams(pManager);

            this.Message = "两点水平距离";
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddLineParameter("Display Line", "L", "Display Line", GH_ParamAccess.list);
            pManager.AddTextParameter("Distance", "D", "Distance", GH_ParamAccess.list) ;
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Point3d> points = new List<Point3d>();
            bool loop = false;
            Plane plane = Plane.WorldXY;

            DA.GetDataList(0, points);
            DA.GetData(1, ref loop);
            DA.GetData(2, ref plane);

            List<Line> displayLines = new List<Line>();

            DA.SetDataList(1, MeasureCalculator.HDistance( points, Decimal, loop, plane, out displayLines));
            DA.SetDataList(0, displayLines);
        }
        #endregion
    }
}