using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace RadiWind.Sort
{
    public class NearlestPointSortComponent : GH_Component
    {

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Properties.Resources._07PointsCloseSort;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("1337c79b-c7c5-495e-a2f6-3eafc19f3039"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;

        /// Initializes a new instance of the NearlestPointSortComponent class.
        /// </summary>
        public NearlestPointSortComponent()
          : base("最近点排序", "最近点排序",
              "最近点排序",
              "RadiWind", "Sort")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("群点", "群点", "群点", GH_ParamAccess.list);
            pManager.AddIntegerParameter("起点Index", "起点Index", "起点Index", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("排序点", "排序点", "排序点", GH_ParamAccess.list);
            pManager.AddIntegerParameter("排序Index", "排序Index", "排序Index", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Point3d> inputPts = new List<Point3d>();
            int index = 0;
            DA.GetDataList(0, inputPts);
            DA.GetData(1, ref index);

            List<SortableItem<Point3d>> NeedToCalculateItems = SortCalculator.GetSortableItems(inputPts);
            List<SortableItem<Point3d>> outItems = new List<SortableItem<Point3d>>() { NeedToCalculateItems[index]};
            NeedToCalculateItems.RemoveAt(index);

            while (NeedToCalculateItems.Count > 0)
            {
                Point3d flagPt = outItems[outItems.Count - 1].Value;

                //Find the point that is nearlist to the flagPt.
                SortableItem<Point3d> nearlistItem = NeedToCalculateItems[0];
                double minDistance = flagPt.DistanceTo(nearlistItem.Value);
                for (int i = 1; i < NeedToCalculateItems.Count; i++)
                {
                    double distance = flagPt.DistanceTo(NeedToCalculateItems[i].Value);
                    if (distance < minDistance)
                    {
                        nearlistItem = NeedToCalculateItems[i];
                        minDistance = distance;
                    }
                }

                //Remove and Add.
                outItems.Add(nearlistItem);
                NeedToCalculateItems.Remove(nearlistItem);
            }

            #region Transform and Retrun it.
            List<int> indexs = new List<int>();
            List<Point3d> points = new List<Point3d>();
            foreach (var item in outItems)
            {
                indexs.Add(item.Index);
                points.Add(item.Value);
            }
            DA.SetDataList(0, points);
            DA.SetDataList(1, indexs);
            #endregion
        }
    }
}