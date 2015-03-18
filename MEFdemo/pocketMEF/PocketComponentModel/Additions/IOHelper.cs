#if (PocketPC || WindowsCE) 
namespace System
{
    #region Using

    using System.Linq;
    using System.Collections.Generic;
    using System.Text;
    using System.Globalization;
    using System.Reflection;
    using System.IO;
    using System.Runtime.InteropServices;

    #endregion // Using    
    
    internal static class IOHelper
    {
		#region Private / Protected Fields

        private static readonly string s_baseDir;

		#endregion // Private / Protected Fields
        
        #region Constructors

        #region Documentation
        /// <summary>
        /// Constructor
        /// </summary>
        #endregion // Documentation
        static IOHelper ()
        {
            s_baseDir = Path.GetDirectoryName(EntryAssemblyPath) + Path.DirectorySeparatorChar;
        }

        #endregion // Constructors

        #region Public Properties

        #region Base Directory

        #region Documentation
        /// <summary>
        /// Gets the base directory
        /// </summary>
        #endregion // Documentation
        public static string BaseDirectory
        {
            get
            {
                return s_baseDir;
            }
        }

        #endregion // Base Directory

        #endregion // Public Properties

        #region Private Methods

        #region Entry Assembly Path

        #region Documentation
        /// <summary>
        /// Gets the entry assembly path.
        /// </summary>
        /// <value></value>
        #endregion
        private static string EntryAssemblyPath
        {
            get
            {
                StringBuilder sb = null;
                IntPtr hModule = GetModuleHandle (IntPtr.Zero);

                if (IntPtr.Zero != hModule)
                {
                    sb = new StringBuilder (255);
                    if (0 == GetModuleFileName (hModule, sb, sb.Capacity))
                    {
                        sb = null;
                    }
                }

                return sb.ToString ();
            }
        }

        #endregion // Entry Assembly Path

        #region Get Module File Name

        #region Documentation
        /// <summary>
        /// Get Module File Name switch between XP and CE implementation.
        /// </summary>
        /// <param name="hModule"></param>
        /// <param name="ModuleName"></param>
        /// <param name="cch"></param>
        /// <returns></returns>
        #endregion
        private static Int32 GetModuleFileName ( IntPtr hModule, StringBuilder
                        ModuleName, Int32 cch )
        {
            try
            {
                return Win32.Reflection.CE_GetModuleFileName (hModule, ModuleName, cch);
            }
            catch (MissingMethodException)
            {
                return Win32.Reflection.XP_GetModuleFileName (hModule, ModuleName, cch);
            }
        }

        #endregion // Get Module File Name

        #region Get Module Handle

        #region Documentation
        /// <summary>
        /// Get Module Handle switch between XP and CE implementation.
        /// </summary>
        /// <param name="ModuleName"></param>
        /// <returns></returns>
        #endregion
        private static IntPtr GetModuleHandle ( IntPtr ModuleName )
        {
            try
            {
                return Win32.Reflection.CE_GetModuleHandle (ModuleName);
            }
            catch (MissingMethodException)
            {
                return Win32.Reflection.XP_GetModuleHandle (ModuleName);
            }
        }

        #endregion // Get Module Handle

        #endregion // Private Methods

        #region Win32

        private class Win32
        {
            public class Reflection
            {
                #region Get Module Handle

                public static IntPtr CE_GetModuleHandle ( IntPtr ModuleName )
                {
                    return Extern.CE_GetModuleHandle (ModuleName);
                }

                public static IntPtr XP_GetModuleHandle ( IntPtr ModuleName )
                {
                    return Extern.XP_GetModuleHandle (ModuleName);
                }

                #endregion // Get Module Handle

                #region Get Module File Name

                public static Int32 CE_GetModuleFileName ( IntPtr hModule,
                    StringBuilder ModuleName, Int32 cch )
                {
                    return Extern.CE_GetModuleFileName (hModule, ModuleName, cch);
                }

                public static Int32 XP_GetModuleFileName ( IntPtr hModule,
                    StringBuilder ModuleName, Int32 cch )
                {
                    return Extern.XP_GetModuleFileName (hModule, ModuleName, cch);
                }

                #endregion // Get Module File Name
            }

            private class Extern
            {
                #region Get Module Handle

                [DllImport ("coredll.dll", SetLastError = true, EntryPoint = "GetModuleHandle")]
                internal static extern IntPtr CE_GetModuleHandle
                    ( IntPtr ModuleName );

                [DllImport ("kernel32.dll", SetLastError = true, EntryPoint = "GetModuleHandle")]
                internal static extern IntPtr XP_GetModuleHandle
                    ( IntPtr ModuleName );

                #endregion // Get Module Handle

                #region Get Module File Name

                [DllImport ("coredll.dll", SetLastError = true, EntryPoint = "GetModuleFileName")]
                internal static extern Int32 CE_GetModuleFileName ( IntPtr hModule,
                    StringBuilder ModuleName, Int32 cch );

                [DllImport ("kernel32.dll", SetLastError = true, EntryPoint = "GetModuleFileName")]
                internal static extern Int32 XP_GetModuleFileName ( IntPtr hModule,
                    StringBuilder ModuleName, Int32 cch );

                #endregion // Get Module File Name
            }
        }

        #endregion // Win32
    }
}
#endif

