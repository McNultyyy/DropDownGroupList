using System;
using System.Collections.Generic;
using System.Linq;

namespace DropDownGroupList
{
    public static class TypeExtension
    {
        public static bool IsEnumerable(this Type type)
        {
            return type.GetInterfaces().Contains(typeof(IEnumerable<>));
        }
    }
}