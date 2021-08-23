/*  Copyright 2021 RadiRhino-秋水, 笑里追风. All Rights Reserved.

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
    public class NumberDecimalsComponent : BaseMeasureComponent
    {
        #region Values
        #region Basic Component info

        public override GH_Exposure Exposure => GH_Exposure.primary;

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override Bitmap Icon => Properties.Resources.NumberDecimal;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("d7679e63-d5a5-47eb-9c84-af396beb06d3");

        #endregion
        #endregion

        /// <summary>
        /// Initializes a new instance of the NumberDecimalsComponent class.
        /// </summary>
        public NumberDecimalsComponent()
          : base("NumberDecimalsComponent", "NumberDecimals", "Description")
        {
        }

        #region Calculate
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Number", "N", "Number", GH_ParamAccess.item);
            base.RegisterInputParams(pManager);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Number", "N", "Number", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            base.SolveInstance(DA);
            string input = "";

            DA.GetData(0, ref input);
            DA.SetData(0, MeasureCalculator.NumberDecimal(input, Decimal));
        }
        #endregion
    }
}