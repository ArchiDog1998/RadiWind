/*  Copyright 2020 RadiRhino-秋水, 笑里追风. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using RadiWindAlgorithm.Sort;
using RadiWindAlgorithm;

namespace RadiWind.Sort
{
    public class NumberTolerancePartionSortComponent : GH_Component
    {
        #region Basic Component Info
        public override GH_Exposure Exposure => GH_Exposure.primary;

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Properties.Resources._01_NumberTolerancePrationSort;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("0415367e-b078-4ea8-904c-442005719f11"); }
        }
        #endregion

        /// <summary>
        /// Initializes a new instance of the NumberTolenrancePartionSortComponent class.
        /// </summary>
        public NumberTolerancePartionSortComponent()
          : base("NumberTolerancePartionSortComponent", "数字容差分组",
              "Description",
              "RadiWind", "Sort")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("数组", "数组", "数组", GH_ParamAccess.list);
            pManager.AddNumberParameter("容差", "容差", "容差", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("排序分组数", "排序分组数", "排序分组数", GH_ParamAccess.tree);
            pManager.AddIntegerParameter("排序Index", "排序Index", "排序Index", GH_ParamAccess.tree);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<double> values = new List<double>();
            double tol = 0;
            DA.GetDataList(0, values);
            DA.GetData(1,ref tol);

            List<List<int>> indexs = new List<List<int>>();
            List<List<double>> result = SortCalculator.NumberTolerancePartitionSort(values, tol, out indexs);

            //Transform it!
            DA.SetDataTree(0, DataTreeHelper.SetDataIntoDataTree<double>(result, this.RunCount - 1));
            DA.SetDataTree(1, DataTreeHelper.SetDataIntoDataTree<int>(indexs, this.RunCount - 1));
        }


    }
}