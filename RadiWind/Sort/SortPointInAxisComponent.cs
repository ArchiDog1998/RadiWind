/*  Copyright 2020 Radino 秋水. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace RadiWind.Sort
{
    public class SortPointInAxisComponent : GH_Component
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
        protected override System.Drawing.Bitmap Icon => Properties.Resources._02PointsAxisSort;

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
          : base("SortPointInAxis", "点单坐标排序",
              "点单坐标排序",
              "RadiWind", "Sort")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        }
        #endregion

        #region Algorithm
        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
        }
        #endregion


    }
}