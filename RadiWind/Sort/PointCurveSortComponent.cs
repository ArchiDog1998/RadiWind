/*  Copyright 2020 RadiRhino-秋水. All Rights Reserved.

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
    public class PointCurveSortComponent : BasePointSortComponent
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
        protected override System.Drawing.Bitmap Icon => Properties.Resources._08PointsCurvesSort1;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("f69d222f-a068-40dc-a001-d356c1b5d1b9");
        #endregion

        #region Basic Component Settings
        /// <summary>
        /// Initializes a new instance of the PointCurveSortComponent class.
        /// </summary>
        public PointCurveSortComponent()
            : base("PointCurveSortComponent", "点多线排序",
                    "点多线排序")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            RegisterPointsInput(pManager);
            pManager.AddCurveParameter("群线", "群线", "群线", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            RegisterPointsOutput(pManager, GH_ParamAccess.list);
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
            List<Curve> inputCrvs = new List<Curve>();
            DA.GetDataList(1, inputCrvs);

            SetSortedPoints(DA, SortCalculator.PointCurveSort(InputPoints, inputCrvs));        }
        #endregion
    }
}