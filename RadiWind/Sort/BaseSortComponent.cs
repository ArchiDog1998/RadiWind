/*  Copyright 2021 RadiRhino-秋水. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using System;
using System.Collections.Generic;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Rhino.Geometry;

namespace RadiWind.Sort
{
    public abstract class BaseSortComponent : RadiWindComponent
    {
        #region Params Layout
        private static string[] _toleranceDesc = { "Tolerance", "t", "Tolerance", };
        private static string[] _basePlaneDesc = { "Base Plane", "p", "Base Plane", }; 
        private static string[] _axisTypeDesc = { "", "T", "AxisType", };
        private static string[] _displayRectDesc = { "Show Rect", "R", "Show Rectangle", };
        private static string[] _sortIndexDesc = { "Sort Index", "I", "SortIndex" };



        #region Tolerance Parameter
        protected void AddTolerParameter(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter(_toleranceDesc[0], _toleranceDesc[1], _toleranceDesc[2], GH_ParamAccess.item, 0.01);
        }

        protected double GetTolerance(IGH_DataAccess DA) => GetParameterValue<double>(DA, _toleranceDesc[0]);
        #endregion

        #region BasePlane Parameter
        protected void AddBasePlaneParameter(GH_InputParamManager pManager)
        {
            pManager.AddPlaneParameter(_basePlaneDesc[0], _basePlaneDesc[1], _basePlaneDesc[2], GH_ParamAccess.item, Plane.WorldXY);
            ((Param_Plane)pManager[pManager.ParamCount - 1]).Hidden = true;
        }

        protected Plane GetBasePlane(IGH_DataAccess DA) => GetParameterValue<Plane>(DA, _basePlaneDesc[0]);
        #endregion

        #region Axis Type Parameter
        private enum AxisType
        {
            X,
            Y,
            Z,
        }

        protected void AddAxisTypeParameter(GH_InputParamManager pManager)
        {
            AddEnumParameter(pManager, _axisTypeDesc[1], _axisTypeDesc[2], AxisType.X);
        }

        protected int GetAxisTypeParameter(IGH_DataAccess DA)=> GetEnumParameter<AxisType>(DA);
        #endregion

        #region Display Rectangle
        protected void AddDisplayRectParameter(GH_OutputParamManager pManager, GH_ParamAccess access)
        {
            pManager.AddRectangleParameter(_displayRectDesc[0], _displayRectDesc[1], _displayRectDesc[2], access);
        }
        protected void SetDisplayRectParameter(IGH_DataAccess DA, IEnumerable<Rectangle3d> rects)
        {
            DA.SetDataList(_displayRectDesc[0], rects);
        }
        protected void SetDisplayRectParameter(IGH_DataAccess DA, DataTree<Rectangle3d> rects)
        {
            DA.SetDataTree(this.Params.IndexOfOutputParam(_displayRectDesc[0]), rects);
        }
        #endregion
        #region Sort Index
        protected void AddSortIndexParameter(GH_OutputParamManager pManager, GH_ParamAccess access)
        {
            pManager.AddIntegerParameter(_sortIndexDesc[0], _sortIndexDesc[1], _sortIndexDesc[2], access);
        }
        protected void AddSortIndexParameter(IGH_DataAccess DA, IEnumerable<int> index)
        {
            DA.SetDataList(_sortIndexDesc[0], index);
        }
        protected void AddSortIndexParameter(IGH_DataAccess DA, DataTree<int> index)
        {
            DA.SetDataTree(this.Params.IndexOfOutputParam(_sortIndexDesc[0]), index);
        }
        #endregion

        #endregion

        /// <summary>
        /// Initializes a new instance of the BaseSortComponent class.
        /// </summary>
        public BaseSortComponent(string name, string nickName, string description)
          : base(name, nickName, description, SubCateName.Sort)
        {
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
            get { return new Guid("BCF8A9FD-1416-4077-8755-8E1E3AA61083"); }
        }
    }
}