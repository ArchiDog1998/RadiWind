using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using RadiWindAlgorithm.Sort;
using Grasshopper.Kernel.Parameters;

namespace RadiWind.Sort
{
    public class PointTolerancePartitionComponent : BaseSortComponent
    {
        /// <summary>
        /// Initializes a new instance of the PointTolerancePartitionComponent class.
        /// </summary>
        public PointTolerancePartitionComponent()
          : base("Point Tolerance Partition", "点分组",
              "点分组")
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Points", "P", "P", GH_ParamAccess.list);
            pManager.AddNumberParameter("Tolerance", "t", "Tolerance", GH_ParamAccess.item, 0.01);

            AddEnumParameter(pManager, "T", "Type", AverageFunction.Major);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("Points", "P", "P", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Point3d> inputPts = new List<Point3d>();
            double tol = 0;
            AverageFunction type = (AverageFunction)GetEnumParameter<AverageFunction>(DA);


            DA.GetData(1, ref tol);
            DA.GetData(2, ref type);

            DA.SetDataList(0, SortCalculator.PointTolerancePartition(inputPts, tol, type));
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
            get { return new Guid("8AC18017-BEC5-4159-8323-9CF5E4808CF8"); }
        }
    }
}