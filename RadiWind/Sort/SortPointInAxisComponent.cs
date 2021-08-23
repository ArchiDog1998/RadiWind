/*  Copyright 2020 Radino 秋水. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using RadiWindAlgorithm.Sort;

namespace RadiWind.Sort
{
    public class SortPointInAxisComponent : BaseSortComponent
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
        protected override System.Drawing.Bitmap Icon => Properties.Resources._02PointsAxisSort1;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("71b207ce-2047-4d8e-bc56-3ec38759a88e");
        #endregion

        #region Basic Component Settings
        /// <summary>
        /// Initializes a new instance of the SortPointInAxis class.
        /// </summary>
        public SortPointInAxisComponent()
          : base("SortPointInAxisComponent", "点单坐标排序",
              "点单坐标排序")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("群点", "群点", "群点", GH_ParamAccess.list);
            pManager.AddPlaneParameter("坐标面", "坐标面", "坐标面", GH_ParamAccess.item, Plane.WorldXY);
            pManager.AddIntegerParameter("轴线选择", "轴线选择", "轴线选择", GH_ParamAccess.item, 0);

            Param_Integer param = pManager[2] as Param_Integer;
            param.AddNamedValue("X", 0);
            param.AddNamedValue("Y", 1);
            param.AddNamedValue("Z", 2);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("排序点", "排序点", "排序点", GH_ParamAccess.list);
            pManager.AddIntegerParameter("排序Index", "排序Index", "排序Index", GH_ParamAccess.list);
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
            int type = 0;

            DA.GetDataList(0, inputPts);
            DA.GetData(1, ref basePlane);
            DA.GetData(2, ref type);

            if (type < 0 || type > 2)
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "轴线选择必须在0-2之间！");

            List<int> indexes = new List<int>();
            List<Point3d> result = SortCalculator.SortPointInAxis(inputPts, type, basePlane, out indexes);

            DA.SetDataList(0, result);
            DA.SetDataList(1, indexes);
        }
        #endregion


    }
}