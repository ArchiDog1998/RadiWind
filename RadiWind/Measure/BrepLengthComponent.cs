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
    public class BrepLengthComponent : GH_Component
    {
        #region Values
        #region Basic Component info

        public override GH_Exposure Exposure => GH_Exposure.quarternary;

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override Bitmap Icon => Properties.Resources.Zp_测量41实体长度;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("a8fcd653-f8d8-4504-98b6-92ec85a16f22");

        #endregion
        #endregion

        /// <summary>
        /// Initializes a new instance of the BrepLengthComponent class.
        /// </summary>
        public BrepLengthComponent()
          : base("BrepLengthComponent", "BrepLength",
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
            pManager.AddBrepParameter("Brep", "B", "Brep", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Decimals", "D", "Decimals", GH_ParamAccess.item, 0);
            pManager.AddBooleanParameter("UseBox", "U", "UseBox", GH_ParamAccess.item, false);

            this.Message = "多重曲面的最长边";
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("Display Curve", "C", "Display Curve", GH_ParamAccess.item);
            pManager.AddTextParameter("Length", "L", "Length", GH_ParamAccess.item);
            pManager.AddBoxParameter("Box", "B", "Box", GH_ParamAccess.item);
            pManager.AddPlaneParameter("Plane", "P", "Plane", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Brep brep = null;
            int decimals = 0;
            bool useBox = false;

            DA.GetData(0, ref brep);
            DA.GetData(1, ref decimals);
            DA.GetData(2, ref useBox);

            Curve displayCurve;
            Box boundingbox;
            Plane plane;
            DA.SetData(1, MeasureCalculator.BrepLength(brep, useBox, out displayCurve, out boundingbox, out plane));
            DA.SetData(0, displayCurve);
            DA.SetData(2, boundingbox);
            DA.SetData(3, plane);
        }
        #endregion
    }
}