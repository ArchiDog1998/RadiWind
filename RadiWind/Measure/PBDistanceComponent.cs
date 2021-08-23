/*  Copyright 2021 RadiRhino-秋水. All Rights Reserved.

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
    public class PBDistanceComponent : BaseMeasureComponent
    {
        #region Values
        #region Basic Component info

        public override GH_Exposure Exposure => GH_Exposure.quarternary;

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override Bitmap Icon => Properties.Resources.PBDistance;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("7cea430e-c67b-4c03-9b23-d6d37ce3b319");

        #endregion
        #endregion

        /// <summary>
        /// Initializes a new instance of the PBDistanceComponent class.
        /// </summary>
        public PBDistanceComponent()
          : base("PBDistanceComponent", "PBDistance", "Description")
        {
        }

        #region Calculate
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Point", "P", "Point", GH_ParamAccess.item);
            pManager.AddPointParameter("Points", "Ps", "Points", GH_ParamAccess.list);
            pManager[1].Optional = true;
            pManager.AddBrepParameter("Brep", "B", "Brep", GH_ParamAccess.item);

            base.RegisterInputParams(pManager);

            this.Message = "点到多重曲面的距离";
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddLineParameter("Display Line", "L", "Display Line", GH_ParamAccess.item);
            pManager.AddTextParameter("Distance", "D", "Distance", GH_ParamAccess.item);
            pManager.AddBrepParameter("Brep", "B", "Brep", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Point3d point = new Point3d();
            List<Point3d> points = new List<Point3d>();
            Brep brep = null;

            DA.GetData(0, ref point);
            DA.GetDataList(1, points);
            DA.GetData(2, ref brep);

            Line displayLine = new Line();
            DA.SetData(1, MeasureCalculator.PBDistance(point, points.ToArray(), ref brep, Decimal, out displayLine, out _));
            DA.SetData(0, displayLine);
            DA.SetData(2, brep);
        }
        #endregion
    }
}