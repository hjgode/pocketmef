#region Using

using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

#endregion // Using

namespace System
{
    internal static class TypeExtensions
    {
        public static Type GetInterface ( this Type instance, string interfaceName, bool ignoreCase )
        {
            foreach (Type type in instance.GetInterfaces ())
            {
                if (string.Compare (  type.Name , interfaceName, ignoreCase ) == 0 )
                {
                    return type;
                }
            }

            return null;
        }
    }
}
