/*  Copyright 2021 RadiRhino-秋水. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace RadiWind.Sort
{
    public abstract class BaseSortComponent : RadiWindComponent
    {
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