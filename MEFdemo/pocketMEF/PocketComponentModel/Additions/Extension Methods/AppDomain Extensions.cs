#region Using

using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

#endregion // Using

namespace System
{
    internal static class AppDomainExtensions
    {
        public static string BaseDirectory ( this AppDomain instance )
        {
    #if (!SILVERLIGHT) && (!PocketPC) 
    #else
    #endif

#if (!PocketPC && !WindowsCE)
            return AppDomain.CurrentDomain.BaseDirectory;
#else
            return IOHelper.BaseDirectory;
#endif
        }
    }
}
