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
    public class PCDistanceComponent : BaseMeasureComponent
    {
        #region Values
        #region Basic Component info

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override Bitmap Icon => Properties.Resources.Zp_测量3;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("01c83803-7c2b-4add-b38b-56794b8fc487");

        #endregion
        #endregion

        /// <summary>
        /// Initializes a new instance of the PCDistanceComponent class.
        /// </summary>
        public PCDistanceComponent()
          : base("PCDistanceComponent", "PCDistance", "Description")
        {
        }

        #region Calculate
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Point", "P", "Point", GH_ParamAccess.item);
            pManager.AddCurveParameter("Curve", "C", "Curve", GH_ParamAccess.item);
            base.RegisterInputParams(pManager);

            this.Message = "点到线的垂距";
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
            Point3d point = new Point3d();
            Curve curve = null;

            DA.GetData(0, ref point);
            DA.GetData(1, ref curve);

            Line displayLine = new Line();
            DA.SetData(1, MeasureCalculator.PCDistance(curve, point, Decimal, out displayLine));
            DA.SetData(0, displayLine);
        }
        #endregion
    }
}