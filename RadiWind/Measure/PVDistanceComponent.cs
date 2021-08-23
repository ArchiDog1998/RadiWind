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
    public class PVDistanceComponent : BaseMeasureComponent
    {
        #region Values
        #region Basic Component info

        public override GH_Exposure Exposure => GH_Exposure.secondary;

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override Bitmap Icon => Properties.Resources.Zp_测量_03点标高;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("adb981e0-a65d-4c8b-be5b-e7ca9f675a72");

        #endregion
        #endregion

        /// <summary>
        /// Initializes a new instance of the PVDistanceComponent class.
        /// </summary>
        public PVDistanceComponent()
          : base("PVDistanceComponent", "High",
              "Description")
        {
        }

        #region Calculate
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Point", "P", "Point", GH_ParamAccess.item);
            pManager.AddPlaneParameter("Plane", "Pl", "Plane", GH_ParamAccess.item, Plane.WorldXY);
            base.RegisterInputParams(pManager);

            this.Message = "点到坐标面的垂距";
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
            Point3d point = Point3d.Origin;
            Plane plane = Plane.WorldXY;

            DA.GetData(0, ref point);
            DA.GetData(1, ref plane);
    
            Line displayLine = new Line();
            DA.SetData(1, MeasureCalculator.PPDistance(point, plane, Decimal, out displayLine));
            DA.SetData(0, displayLine);
        }
        #endregion
    }
}