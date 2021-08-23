using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace RadiWind.Measure
{
    public class BaseMeasureComponent : DefaultDecimalComponent
    {
        /// <summary>
        /// Initializes a new instance of the DecimalableComponent class.
        /// </summary>
        public BaseMeasureComponent(string name, string nickName, string description)
          : base(name, nickName, description)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("Decimal", "D", "Decimal", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            int rightDecimal = 0;
            if (DA.GetData("Decimal", ref rightDecimal))
                ChangeDecimal(rightDecimal);
            else ChangeDecimal(null);
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
            get { return new Guid("3BAD77CE-E65D-4660-84EB-5A34A245E01F"); }
        }
    }
}