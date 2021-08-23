/*  Copyright 2021 RadiRhino-秋水. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace RadiWind.Measure
{
    public class DefaultDecimalComponent : RadiWindComponent
    {
        private static int _defaultDecimal = 2;

        private int? _rightDecimal = null;

        public int Decimal => _rightDecimal.HasValue ? _rightDecimal.Value : _defaultDecimal;

        /// <summary>
        /// Initializes a new instance of the DecimalableComponent class.
        /// </summary>
        public DefaultDecimalComponent()
          : this("DecimalableComponent", "DecimalableComponent", "Description")
        {
        }

        protected DefaultDecimalComponent(string name, string nickName, string description)
            :base(name, nickName, description, SubCateName.Measure)
        {

        }

        protected void ChangeDecimal(int? rightDecial)
        {
            _rightDecimal = rightDecial;
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("DefaultDecimal", "DD", "DefaultDecimal", GH_ParamAccess.item, 2);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            int decimals = 2;
            DA.GetData(0, ref decimals);
            _defaultDecimal = decimals;
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("22D75762-C80A-4D8E-BA4C-F615BF410AA0"); }
        }
    }
}