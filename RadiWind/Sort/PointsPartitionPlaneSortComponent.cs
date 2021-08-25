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
    public class PointsPartitionPlaneSortComponent : BasePointSortComponent
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
        protected override System.Drawing.Bitmap Icon => Properties.Resources._05PointsPartionPlaneSort1;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("9280d303-e4d1-4648-9720-9970c6b882a5");
        #endregion

        #region Basic Component Settings
        /// <summary>
        /// Initializes a new instance of the PointsPartitionPlaneSortComponent class.
        /// </summary>
        public PointsPartitionPlaneSortComponent()
            : base("PointsPartitionPlaneSortComponent", "点平面容差分组排序",
                    "点平面容差分组排序")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            base.RegisterInputParams(pManager);
            AddBasePlaneParameter(pManager);
            pManager.AddNumberParameter("x容差", "x容差", "x容差", GH_ParamAccess.item, 0.01);
            pManager.AddNumberParameter("y容差", "y容差", "y容差", GH_ParamAccess.item, 0.01);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            RegisterPointsOutput(pManager, GH_ParamAccess.tree);
            AddDisplayRectParameter(pManager, GH_ParamAccess.tree);
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

            Plane basePlane = GetBasePlane(DA);
            double xTol = 0;
            double yTol = 0;

            DA.GetData(2, ref xTol);
            DA.GetData(3, ref yTol);

            List<List<Rectangle3d>> showRect = new List<List<Rectangle3d>>();
            SetSortedPoints(DA, SortCalculator.XYPartitionSortedByX(InputPoints, basePlane, xTol, yTol, out showRect));
            SetDisplayRectParameter(DA, DataTreeHelper.SetDataIntoDataTree<Rectangle3d>(showRect, this.RunCount - 1));
        }
        #endregion
    }
}