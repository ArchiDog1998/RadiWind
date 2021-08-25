/*  Copyright 2021 RadiRhino-秋水, 笑里追风. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using RadiWindAlgorithm.Measure;
using Rhino.Geometry;

namespace RadiWind.Measure
{
    public class VDistanceComponent : BaseMeasureComponent
    {
        public override GH_Exposure Exposure => GH_Exposure.secondary;

        /// <summary>
        /// Initializes a new instance of the VDistanceComponent class.
        /// </summary>
        public VDistanceComponent()
          : base("VDistanceComponent", "VDistance", "Description")
        {
        }

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

            this.Message = "两点垂直距离";
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
            List<Point3d> points = new List<Point3d>();
            bool loop = false;
            Plane plane = Plane.WorldXY;

            DA.GetDataList(0, points);
            DA.GetData(1, ref loop);
            DA.GetData(2, ref plane);

            List<Line> displayLines = new List<Line>();

            DA.SetDataList(1, MeasureCalculator.VDistance(points, Decimal, loop, plane, out displayLines));
            DA.SetDataList(0, displayLines);
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return Properties.Resources.HeightIcon;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("41FD4F6A-8954-4E43-8C28-C35A84F93657"); }
        }
    }
}