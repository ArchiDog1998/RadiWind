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
    public class CCAngleComponent : GH_Component
    {
        #region Values
        #region Basic Component info

        public override GH_Exposure Exposure => GH_Exposure.quarternary;

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override Bitmap Icon => Properties.Resources.Zp_测量51线最近点切线夹角;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("0572fda8-72f9-49a0-93f5-9c4f070ef8a5");

        #endregion
        #endregion

        /// <summary>
        /// Initializes a new instance of the CCAngleComponent class.
        /// </summary>
        public CCAngleComponent()
          : base("CCAngleComponent", "CCAngle",
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
            pManager.AddCurveParameter("Curve A", "A", "Curve A", GH_ParamAccess.item);
            pManager.AddCurveParameter("Curve B", "B", "Curve B", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Decimals", "D", "Decimals", GH_ParamAccess.item, 0);

            this.Message = "线线切方向夹角";
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("Point A", "PA", "Point A", GH_ParamAccess.item);
            pManager.AddPointParameter("Point B", "PB", "Point B", GH_ParamAccess.item);
            pManager.AddVectorParameter("Vector A", "VA", "Vector A", GH_ParamAccess.item);
            pManager.AddVectorParameter("Vector B", "VB", "Vector B", GH_ParamAccess.item);
            pManager.AddTextParameter("Angle", "A", "Angle", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Curve curve1 = null;
            Curve curve2 = null;
            int decimals = 0;

            DA.GetData(0, ref curve1);
            DA.GetData(1, ref curve2);
            DA.GetData(2, ref decimals);

            Point3d pt1, pt2;
            Vector3d vec1, vec2;
            //string ang;

            DA.SetData(4, MeasureCalculator.CCAngle(curve1, curve2, decimals, out pt1, out pt2, out vec1, out vec2));
            DA.SetData(0, pt1);
            DA.SetData(1, pt2);
            DA.SetData(2, vec1);
            DA.SetData(3, vec2);
        }
        #endregion
    }
}