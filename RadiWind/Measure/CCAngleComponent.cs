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
    public class CCAngleComponent : BaseMeasureComponent
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
          : base("CCAngleComponent", "CCAngle", "Description")
        {
        }

        #region Calculate
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Curves", "C", "Curves", GH_ParamAccess.list);
            pManager.AddBooleanParameter("Loop", "L", "Loop", GH_ParamAccess.item, false);
            base.RegisterInputParams(pManager);

            this.Message = "线线切方向夹角";
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("Point A", "PA", "Point A", GH_ParamAccess.list);
            pManager.AddPointParameter("Point B", "PB", "Point B", GH_ParamAccess.list);
            pManager.AddVectorParameter("Vector A", "VA", "Vector A", GH_ParamAccess.list);
            pManager.AddVectorParameter("Vector B", "VB", "Vector B", GH_ParamAccess.list);
            pManager.AddTextParameter("Angle", "A", "Angle", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Curve> crvs = new List<Curve>();
            bool loop = false;

            DA.GetDataList(0, crvs);
            DA.GetData(1, ref loop);

            List<Point3d> pt1, pt2;
            List<Vector3d> vec1, vec2;

            DA.SetDataList(4, MeasureCalculator.CCAngle(crvs, loop, Decimal, out pt1, out pt2, out vec1, out vec2));
            DA.SetDataList(0, pt1);
            DA.SetDataList(1, pt2);
            DA.SetDataList(2, vec1);
            DA.SetDataList(3, vec2);
        }
        #endregion
    }
}