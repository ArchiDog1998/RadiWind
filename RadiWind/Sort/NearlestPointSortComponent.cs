/*  Copyright 2020 RadiRhino-秋水, 笑里追风. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using RadiWindAlgorithm.Sort;

namespace RadiWind.Sort
{
    public class NearlestPointSortComponent : GH_Component
    {

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Properties.Resources._07PointsCloseSort1;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("1337c79b-c7c5-495e-a2f6-3eafc19f3039"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;

        /// Initializes a new instance of the NearlestPointSortComponent class.
        /// </summary>
        public NearlestPointSortComponent()
          : base("Nearlest Point Sort", "最近点排序",
              "最近点排序",
              "RadiWind", "Sort")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("群点", "群点", "群点", GH_ParamAccess.list);
            pManager.AddIntegerParameter("起点Index", "起点Index", "起点Index", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("排序点", "排序点", "排序点", GH_ParamAccess.list);
            pManager.AddIntegerParameter("排序Index", "排序Index", "排序Index", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Point3d> inputPts = new List<Point3d>();
            int index = 0;
            DA.GetDataList(0, inputPts);
            DA.GetData(1, ref index);

            List<int> indexes = new List<int>();
            List<Point3d> points = SortCalculator.NearlestPointSortByIndex(inputPts, index, out indexes);

            DA.SetDataList(0, points);
            DA.SetDataList(1, indexes);
        }
    }
}