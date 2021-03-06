﻿using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Dfc.CourseDirectory.Web.Helpers
{
    public static class WebHelper
    {
        public static string GetEnumDescription(Enum value)
        {
            return
                value
                    .GetType()
                    .GetMember(value.ToString())
                    .FirstOrDefault()
                    ?.GetCustomAttribute<DescriptionAttribute>()
                    ?.Description;
        }
    }
}
