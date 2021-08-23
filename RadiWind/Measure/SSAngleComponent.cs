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
    public class SSAngleComponent : BaseMeasureComponent
    {
        #region Values
        #region Basic Component info

        public override GH_Exposure Exposure => GH_Exposure.quarternary;

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override Bitmap Icon => Properties.Resources.Zp_测量52面面夹角;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("2441af1c-b057-4d4b-b495-bde24334b9ee");


        #endregion
        #endregion

        /// <summary>
        /// Initializes a new instance of the SSAngleComponent class.
        /// </summary>
        public SSAngleComponent()
          : base("SSAngleComponent", "SSAngle",
              "Description")
        {
        }

        #region Calculate
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddSurfaceParameter("Surface A", "A", "Surface A", GH_ParamAccess.item);
            pManager.AddSurfaceParameter("Surface B", "B", "Surface B", GH_ParamAccess.item);
            base.RegisterInputParams(pManager);

            this.Message = "面到面的夹角";
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPlaneParameter("Base Plane", "bP", "Base Plane", GH_ParamAccess.item);
            pManager.AddPlaneParameter("Plane A", "PA", "Plane A", GH_ParamAccess.item);
            pManager.AddPlaneParameter("Plane B", "PB", "Plane B", GH_ParamAccess.item);
            pManager.AddTextParameter("Angle", "A", "Angle", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Surface srf1 = default(Surface);
            Surface srf2 = default(Surface);

            DA.GetData(0, ref srf1);
            DA.GetData(1, ref srf2);

            Plane bpl, pl1, pl2;
            DA.SetData(3, MeasureCalculator.SSAngle(srf1, srf2, Decimal, out bpl, out pl1, out pl2));
            DA.SetData(0, bpl);
            DA.SetData(1, pl1);
            DA.SetData(2, pl2);
        }
        #endregion
    }
}