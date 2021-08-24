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

        protected T GetParameterValue<T>(IGH_DataAccess DA, string name)
        {
            T value = default(T);
            if (!DA.GetData(name, ref value)) throw new ArgumentNullException(name, $"{name} Parameter can NOT be a null value.");
            return value;
        }
        #region EnumParameter
        protected void AddEnumParameter<T>(GH_Component.GH_InputParamManager pManager, string nickName, string descrption, T defaultValue) where T:Enum
        {
            // Check if T is a Enum.
            if (!typeof(Enum).IsAssignableFrom(typeof(T))) throw new ArgumentOutOfRangeException(typeof(T).ToString(), "It must be a Enum Type!");

            //Add to Input Param.
            pManager.AddIntegerParameter(typeof(T).ToString(), nickName, descrption, GH_ParamAccess.item, (int)(object)defaultValue);

            //Add Description and NamedValue.
            Param_Integer param = pManager[pManager.ParamCount - 1] as Param_Integer;
            foreach (var item in Enum.GetValues(typeof(T)))
            {
                param.AddNamedValue(item.ToString(), (int)item);
                param.Description += $"\n{item} = {(int)item},";
            }
        }

        protected int GetEnumParameter<T>(IGH_DataAccess DA)where T:Enum
        {
            int value = GetParameterValue<int>(DA, typeof(T).ToString());
            if (!Enum.IsDefined(typeof(T), value)) throw new ArgumentOutOfRangeException(typeof(T).ToString());
            return value;
        }
        #endregion

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("2F099C58-C5BF-4E80-A7D6-C9F8A6660CC9"); }
        }
    }
}