#region Using

using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

#endregion // Using

namespace System
{
    internal static class StringExtensions
    {
        public static string ToUpperInvariant ( this string instance )
        {
            return instance.ToUpper (CultureInfo.InvariantCulture);
        }
    }
}
