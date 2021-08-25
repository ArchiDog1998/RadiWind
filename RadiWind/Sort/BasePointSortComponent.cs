using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using RadiWindAlgorithm.Sort;
using RadiWindAlgorithm;

namespace RadiWind.Sort
{
    public abstract class BasePointSortComponent : BaseSortComponent
    {
        protected List<Point3d> InputPoints { get; private set; }

        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public BasePointSortComponent(string name, string nickname, string description)
          : base(name, nickname, description)
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Points", "P", "Points", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("Points", "P", "Points", GH_ParamAccess.list);
        }

        /// <summary>
        /// Add two output Parameters.
        /// </summary>
        /// <param name="pManager"></param>
        /// <param name="access"></param>
        protected void RegisterPointsOutput(GH_OutputParamManager pManager, GH_ParamAccess access)
        {
            pManager.AddPointParameter("Points", "P", "Points", access);
            pManager.AddIntegerParameter("Index", "I", "Index", access);
        }

        protected void CollectPoints(IGH_DataAccess DA)
        {
            List<Point3d> pts = new List<Point3d>();
            DA.GetDataList("Points", pts);
            this.InputPoints = pts;
        }

        protected void SetSortedPoints(IGH_DataAccess DA, List<SortableItem<Point3d>> SortedPts)
        {
            List<int> index = new List<int>();
            List<Point3d> sortPts = SortCalculator.DispatchIt(SortedPts, out index);
            DA.SetDataList("Points", sortPts);
            DA.SetDataList("Index", index);
        }

        protected void SetSortedPoints(IGH_DataAccess DA, List<List<SortableItem<Point3d>>> SortedPts)
        {
            List<List<int>> index = new List<List<int>>();
            List<List<Point3d>> sortPts = SortCalculator.DispatchIt(SortedPts, out index);
            DA.SetDataTree(this.Params.IndexOfOutputParam( "Points"), DataTreeHelper.SetDataIntoDataTree(sortPts, this.RunCount - 1));
            DA.SetDataTree(this.Params.IndexOfOutputParam("Index"), DataTreeHelper.SetDataIntoDataTree(index, this.RunCount - 1));
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
            get { return new Guid("587A84ED-B8A3-4B59-A087-5D6F5EDEAD12"); }
        }
    }
}