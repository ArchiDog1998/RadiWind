/*  Copyright 2020 RadiRhino-秋水, 笑里追风. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using RadiWindAlgorithm;
using RadiWindAlgorithm.Sort;
using Grasshopper;

namespace RadiWind.Sort
{
    public class SortPointInAxisWithToleranceComponent : BasePointSortComponent
    {
        #region Basic Component Info
        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Properties.Resources._03PointsPartionAxisSort1;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("7c43b719-2e3e-4e9e-b875-3237f3c9f50e"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;
        #endregion

        /// <summary>
        /// Initializes a new instance of the SortPointInAxisWithToleranceComponent class.
        /// </summary>
        public SortPointInAxisWithToleranceComponent()
          : base("SortPointInAxisWithToleranceComponent", "点但坐标容差排序",
              "点但坐标容差排序")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            base.RegisterInputParams(pManager);
            AddBasePlaneParameter(pManager);
            AddTolerParameter(pManager);
            AddAxisTypeParameter(pManager);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            RegisterPointsOutput(pManager, GH_ParamAccess.tree);
            AddDisplayRectParameter(pManager, GH_ParamAccess.tree);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            CollectPoints(DA);
            List<List<Rectangle3d>> showRects;
            SetSortedPoints(DA, SortCalculator.SortPointInAxisWithTolerance(InputPoints, GetAxisTypeParameter(DA), GetBasePlane(DA), GetTolerance(DA), out showRects));
            SetDisplayRectParameter(DA, DataTreeHelper.SetDataIntoDataTree(showRects, this.RunCount - 1));

        }


    }
}