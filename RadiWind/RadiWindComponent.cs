/*  Copyright 2021 RadiRhino-秋水. All Rights Reserved.

    Distributed under MIT license.

    See file LICENSE for detail or copy at http://opensource.org/licenses/MIT
*/

using System;
using System.Linq;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Grasshopper.Kernel.Parameters;

namespace RadiWind
{
    public abstract class RadiWindComponent : GH_Component
    {
        protected enum SubCateName
        {
            Measure,
            Sort,
        }

        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        protected RadiWindComponent(string name, string nickName, string description, SubCateName cateName)
          : base(name, nickName, description, "RadiWind", cateName.ToString())
        {
        }

        protected void AddIntegerParameter(GH_Component.GH_InputParamManager pManager, string name, string nickName, string descrption, GH_ParamAccess access, Type enumType, int? defaultValue = null)
        {
            if (!typeof(Enum).IsAssignableFrom(enumType)) throw new ArgumentOutOfRangeException(nameof(enumType), "It must be a Enum Type!");

            if (defaultValue.HasValue)
                pManager.AddIntegerParameter(name, nickName, descrption, access, defaultValue.Value);
            else
                pManager.AddIntegerParameter(name, nickName, descrption, access);


            Param_Integer param = pManager[pManager.ParamCount - 1] as Param_Integer;
            foreach (var item in Enum.GetValues(enumType))
            {
                param.AddNamedValue(item.ToString(), (int)item);
                param.Description += $"\n{item} = {(int)item},";
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("2F099C58-C5BF-4E80-A7D6-C9F8A6660CC9"); }
        }
    }
}