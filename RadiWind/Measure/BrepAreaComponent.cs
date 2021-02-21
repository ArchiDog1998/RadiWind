/*  Copyright 2021 RadiRhino-秋水. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;
using RadiWindAlgorithm.Measure;
using Grasshopper.Kernel.Types;

namespace RadiWind.Measure
{
    public class BrepAreaComponent : GH_Component
    {
        #region Values
        #region Basic Component info

        public override GH_Exposure Exposure => GH_Exposure.quinary;

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override Bitmap Icon => Properties.Resources.Zp_测量61面积测量;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("3d9f26c8-9383-4ae5-8961-7bf2fddd3a80");

        #endregion
        #endregion

        /// <summary>
        /// Initializes a new instance of the BrepAreaComponent class.
        /// </summary>
        public BrepAreaComponent()
          : base("BrepAreaComponent", "BrepArea",
              "Description",
              "RadiWind", "Measure")
        {
        }

        #region Calculate
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBrepParameter("Brep", "B", "Brep", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Decimals", "D", "Decimals", GH_ParamAccess.item, 0);

            Param_Integer param = new Param_Integer();
            foreach (int key in MeasureCalculator.Unit.Keys)
            {
                param.AddNamedValue(MeasureCalculator.Unit[key].Name, key);
            }
            param.SetPersistentData(new GH_Integer(0));
            pManager.AddParameter(param, "Unit", "U", "Unit", GH_ParamAccess.item);

            this.Message = "多重曲面面积";
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Area", "A", "Area", GH_ParamAccess.item);
            pManager.AddTextParameter("Unit", "U", "Unit", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Brep brep = null;
            int decimals = 0;
            int unit = 0;

            DA.GetData(0, ref brep);
            DA.GetData(1, ref decimals);
            DA.GetData(2, ref unit);

            if (!MeasureCalculator.Unit.ContainsKey(unit))
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Unit 必须要在区间内！");
                unit = 0;
            }
            DA.SetData(0, MeasureCalculator.BrepArea(brep, decimals, unit));
            DA.SetData(1, MeasureCalculator.Unit[unit].Name);
        }
        #endregion
    }
}