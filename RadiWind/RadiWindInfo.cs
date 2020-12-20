﻿using Grasshopper.Kernel;
using System;
using System.Drawing;

namespace RadiWind
{
    public class RadiWindInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "RadiWind";
            }
        }
        public override Bitmap Icon
        {
            get
            {
                //Return a 24x24 pixel bitmap to represent this GHA library.
                return null;
            }
        }
        public override string Description
        {
            get
            {
                //Return a short string describing the purpose of this GHA library.
                return "RadiWind for 笑里追风";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("75af6205-994d-44eb-a9e4-f7a3de0ac2e8");
            }
        }

        public override string AuthorName
        {
            get
            {
                //Return a string identifying you or your company.
                return "秋水 笑里追风";
            }
        }
        public override string AuthorContact
        {
            get
            {
                //Return a string representing your preferred contact details.
                return "1123993881@qq.com";
            }
        }
    }
}
