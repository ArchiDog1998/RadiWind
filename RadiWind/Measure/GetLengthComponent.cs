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
    public class GetLengthComponent : BaseMeasureComponent
    {
        #region Values
        #region Basic Component info

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override Bitmap Icon => Properties.Resources.GetLength;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("c473cb9e-497a-4020-8987-b25fb90e0332");

        #endregion
        #endregion

        /// <summary>
        /// Initializes a new instance of the GetLengthComponent class.
        /// </summary>
        public GetLengthComponent()
          : base("GetLengthComponent", "GetLength","Description")
        {
        }

        #region Calculate
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve", "C", "Cureve", GH_ParamAccess.item);
            base.RegisterInputParams(pManager);

            this.Message = "线长度及两端点";
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("Start Point", "S", "Start Point", GH_ParamAccess.item);
            pManager.AddPointParameter("End Point", "E", "End Point", GH_ParamAccess.item);
            pManager.AddTextParameter("Length", "L", "Length", GH_ParamAccess.item);

        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Curve curve = null;

            DA.GetData(0, ref curve);

            Point3d startPt = new Point3d();
            Point3d endPt = new Point3d();
            DA.SetData(2, MeasureCalculator.GetLength(curve, Decimal, out startPt, out endPt));
            DA.SetData(0, startPt);
            DA.SetData(1, endPt);
        }
        #endregion
    }
}