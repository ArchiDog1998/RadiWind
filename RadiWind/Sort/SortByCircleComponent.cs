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
    public class SortByCircleComponent : GH_Component
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
        protected override System.Drawing.Bitmap Icon => Properties.Resources._06PointsCircleSort;

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
                    "环绕排序",
                    "RadiWind", "Sort")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("群点", "群点", "群点", GH_ParamAccess.list);
            pManager.AddPlaneParameter("坐标面", "坐标面", "坐标面", GH_ParamAccess.item, Plane.WorldXY);
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
            pManager.AddPointParameter("排序点", "排序点", "排序点", GH_ParamAccess.list);
            pManager.AddIntegerParameter("排序Index", "排序Index", "排序Index", GH_ParamAccess.list);
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
            List<Point3d> inputPts = new List<Point3d>();
            Plane basePlane = Plane.WorldXY;
            double radius = 0;

            DA.GetDataList(0, inputPts);
            DA.GetData(1, ref basePlane);
            DA.GetData(2, ref radius);

            //Change Degree.
            if (((Param_Number)this.Params.Input[2]).UseDegrees)
            {
                radius = RhinoMath.ToRadians(radius);
            }

            Line showLine;
            Plane showPlane;
            List<int> indexes;
            List<Point3d> outPts = SortCalculator.SortByCircle(inputPts, basePlane, radius, out showLine, out showPlane, out indexes);

            DA.SetDataList(0, outPts);
            DA.SetDataList(1, indexes);
            DA.SetData(2, showLine);
            DA.SetData(3, showPlane);
        }
        #endregion
    }
}