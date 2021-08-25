/*  Copyright 2020 RadiRhino-秋水. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Rhino;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using RadiWindAlgorithm.Sort;

namespace RadiWind.Sort
{
    public class SortByCircleComponent : BasePointSortComponent
    {
        #region Basic Component Info

        /// <summary>
        /// Change The Exposure like slash in the same subcategory on component tab.
        /// </summary>
        public override GH_Exposure Exposure => GH_Exposure.secondary;

        /// <summary>
        /// Provides an Icon for the component.
        ///You can add image files to your project resources and access them like this:
        /// return Resources.IconForThisComponent;
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Properties.Resources._06PointsCircleSort1;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("7b9cc71b-0a4e-4c6d-952a-91c0b77bb892");
        #endregion

        #region Basic Component Settings
        /// <summary>
        /// Initializes a new instance of the SortByCircle class.
        /// </summary>
        public SortByCircleComponent()
            : base("SortByCircleComponent", "环绕排序",
                    "环绕排序")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            base.RegisterInputParams(pManager);
            AddBasePlaneParameter(pManager);
            pManager.AddAngleParameter("旋转角度", "旋转角度", "旋转角度", GH_ParamAccess.item, -180);

            //Make the default is use degree.
            Param_Number param = pManager[2] as Param_Number;
            param.UseDegrees = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            RegisterPointsOutput(pManager, GH_ParamAccess.list);
            pManager.AddLineParameter("起止线", "起止线", "起止线", GH_ParamAccess.item);
            pManager.AddPlaneParameter("坐标平面", "坐标平面", "坐标平面", GH_ParamAccess.item);
        }
        #endregion

        #region Algorithm
        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            CollectPoints(DA);

            double radius = 0;
            DA.GetData(2, ref radius);
            //Change Degree.
            if (((Param_Number)this.Params.Input[2]).UseDegrees)
            {
                radius = RhinoMath.ToRadians(radius);
            }

            Line showLine;
            Plane showPlane;
            SetSortedPoints(DA, SortCalculator.SortByCircle(InputPoints, GetBasePlane(DA), radius, out showLine, out showPlane));
            DA.SetData(2, showLine);
            DA.SetData(3, showPlane);
        }
        #endregion
    }
}