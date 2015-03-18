// -----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
// -----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Internal;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Diagnostics;

namespace System.ComponentModel.Composition
{
    internal static class MetadataViewProvider
    {
        private const string EXC__MISSING_PROVIDER =
@"Unregistered tyep ({0}), 
PocketMEF can only convert metadata that has register provider.
Use the following methos in order to register MetadataViewProvider:
MetadataViewProvider.RegisterProvider<TMetadataView>(IMetadataViewProvider<TMetadataView> provider)
MetadataViewProvider.RegisterProvider<{1}>(IMetadataViewProvider<{1}> provider)";

        private static Dictionary<Type, object> s_providers = new Dictionary<Type, object>();
        
        #region Constructors

        #region Documentation
        /// <summary>
        /// Static Constructor
        /// </summary>
        #endregion // Documentation
        static MetadataViewProvider ()
        {
            RegisterProvider<IDictionary<string, object>>(MetadataViewProvider_Default.Instance);
            RegisterProvider<Dictionary<string, object>>(MetadataViewProvider_Default_Type.Instance);
            RegisterProvider<IEnumerable<KeyValuePair<string, object>>>(MetadataViewProvider_Default_IEnumerable.Instance);
        }

        #endregion // Constructors

        #region Documentation
        /// <summary>
        /// Add metadata provider
        /// </summary>
        /// <typeparam name="TMetadataView"></typeparam>
        /// <param name="provider"></param>
        #endregion // Documentation   
        public static void RegisterProvider<TMetadataView>(IMetadataViewProvider<TMetadataView> provider)
        {
            var type = typeof(TMetadataView);
            if (s_providers.ContainsKey(type))
            {
                s_providers.Remove(type);
            }
            s_providers.Add(type, provider);
        }

        #region Documentation
        /// <summary>
        /// Remove metadata provider
        /// </summary>
        /// <typeparam name="TMetadataView"></typeparam>
        #endregion // Documentation   
        public static void UnregisterProvider<TMetadataView>()
        {
            s_providers.Remove(typeof(TMetadataView));
        }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        #region Documentation
        /// <summary>
        /// .NET compact framework does not use reflection in order of converting the metadata into typed instance
        /// only providers that previously registered are valid
        /// </summary>
        /// <typeparam name="TMetadataView"></typeparam>
        /// <param name="metadata"></param>
        /// <returns></returns>
        #endregion // Documentation
        public static TMetadataView GetMetadataView<TMetadataView>(IDictionary<string, object> metadata)
        {
            IMetadataViewProvider<TMetadataView> provider = null;
            if (s_providers.ContainsKey(typeof(TMetadataView)))
                provider = (IMetadataViewProvider<TMetadataView>)s_providers[typeof(TMetadataView)];

            if (provider == null)
                throw new CompositionContractMismatchException(string.Format(EXC__MISSING_PROVIDER, typeof(TMetadataView).FullName, typeof(TMetadataView).Name));

            return provider.GetMetadataView (metadata);
            //Type metadataViewType = typeof(TMetadataView);

            //// If the Metadata dictionary is cast compatible with the passed in type
            //if (metadataViewType.IsAssignableFrom(typeof(IDictionary<string, object>)))
            //{
            //    return (TMetadataView)metadata;
            //}
            //// otherwise is it a metadata view
            //else
            //{
            //    Type proxyType;
            //    if (metadataViewType.IsInterface)
            //    {
            //        try
            //        {
            //            proxyType = MetadataViewGenerator.GenerateView(metadataViewType);
            //        }
            //        catch (TypeLoadException ex)
            //        {
            //            throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, Strings.NotSupportedInterfaceMetadataView, metadataViewType.FullName), ex);
            //        }
            //    }
            //    else
            //    {
            //        proxyType = metadataViewType;
            //    }

            //    // Now we have the type for the proxy create it
            //    try
            //    {
            //        return (TMetadataView)Activator.CreateInstance(proxyType, new object[] { metadata });
            //    }
            //    catch (MissingMethodException ex)
            //    {
            //        // Unable to create an Instance of the Metadata view '{0}' because a constructor could not be selected.  Ensure that the type implements a constructor which takes an argument of type IDictionary<string, object>.
            //        throw new CompositionContractMismatchException(string.Format(CultureInfo.CurrentCulture,
            //            Strings.CompositionException_MetadataViewInvalidConstructor,
            //            proxyType.AssemblyQualifiedName), ex);
            //    }
            //    catch (TargetInvocationException ex)
            //    {
            //        //Unwrap known failures that we want to present as CompositionContractMismatchException
            //        if(metadataViewType.IsInterface)
            //        {
            //            if(ex.InnerException.GetType() == typeof(InvalidCastException))
            //            {
            //                // Unable to create an Instance of the Metadata view {0} because the exporter exported the metadata for the item {1} with the value {2} as type {3} but the view imports it as type {4}.
            //                throw new CompositionContractMismatchException(string.Format(CultureInfo.CurrentCulture, 
            //                    Strings.ContractMismatch_InvalidCastOnMetadataField,
            //                    ex.InnerException.Data[MetadataViewGenerator.MetadataViewType],
            //                    ex.InnerException.Data[MetadataViewGenerator.MetadataItemKey],
            //                    ex.InnerException.Data[MetadataViewGenerator.MetadataItemValue],
            //                    ex.InnerException.Data[MetadataViewGenerator.MetadataItemSourceType],
            //                    ex.InnerException.Data[MetadataViewGenerator.MetadataItemTargetType]), ex);
            //            }
            //            else if (ex.InnerException.GetType() == typeof(NullReferenceException))
            //            {
            //                // Unable to create an Instance of the Metadata view {0} because the exporter exported the metadata for the item {1} with a null value and null is not a valid value for type {2}.
            //                throw new CompositionContractMismatchException(string.Format(CultureInfo.CurrentCulture,
            //                    Strings.ContractMismatch_NullReferenceOnMetadataField,
            //                    ex.InnerException.Data[MetadataViewGenerator.MetadataViewType],
            //                    ex.InnerException.Data[MetadataViewGenerator.MetadataItemKey],
            //                    ex.InnerException.Data[MetadataViewGenerator.MetadataItemTargetType]), ex);
            //            }
            //        }
            //        throw;
            //    }
            //}
        }

        public static bool IsViewTypeValid(Type metadataViewType)
        {
            Assumes.NotNull(metadataViewType);

            // If the Metadata dictionary is cast compatible with the passed in type
            if (ExportServices.IsDefaultMetadataViewType(metadataViewType)
            ||  metadataViewType.IsInterface
            ||  ExportServices.IsDictionaryConstructorViewType(metadataViewType))
            {
                return true;
            }

            return false;
        }
        
		#region Nested Types

        #region MetadataViewProvider_Default

        #region Documentation
        /// <summary>
        /// Singleton implementation of IMetadataViewProvider<IDictionary<string,object>>
        /// </summary>
        #endregion // Documentation
        private class MetadataViewProvider_Default : IMetadataViewProvider<IDictionary<string,object>>
        {
            #region Private / Protected Fields

            private static MetadataViewProvider_Default s_instance;
            private static Exception s_initialException = null;

            #endregion // Private / Protected Fields

            #region Constructor

            #region Documentation
            /// <summary>
            /// Constructor
            /// </summary>
            #endregion
            private MetadataViewProvider_Default()
            {
            }

            #region Static Constructor

            #region Documentation
            /// <summary>
            /// This class follow the singleton pattern
            /// </summary>
            #endregion
            static MetadataViewProvider_Default()
            {
                try
                {
                    s_instance = new MetadataViewProvider_Default();
                }
                #region Exception Handling
                catch (SecurityException exc)
                {
                    s_initialException = exc;
                }
                catch (Exception exc)
                {
                    s_initialException = exc;
                }
                #endregion // Exception Handling
            }

            #endregion // Static Constructor

            #endregion // Constructor

            #region Public Static Propertis

            #region Instance

            #region Documentation
            /// <summary>
            /// Return the singleton instance.
            /// </summary>
            #endregion
            public static MetadataViewProvider_Default Instance
            {
                get
                {
                    if (s_initialException != null)
                        throw s_initialException;

                    return s_instance;
                }
            }

            #endregion // Instance

            #endregion // Public Static Propertis

            #region IMetadataViewProvider<IMetadataView> Members

            public IDictionary<string, object> GetMetadataView(IDictionary<string, object> metadata)
            {
                return metadata;
            }

            #endregion
        }

        #endregion // MetadataViewProvider_Default

        #region MetadataViewProvider_Default_Type

        #region Documentation
        /// <summary>
        /// Singleton implementation of IMetadataViewProvider<Dictionary<string,object>>
        /// </summary>
        #endregion // Documentation
        private class MetadataViewProvider_Default_Type : IMetadataViewProvider<Dictionary<string,object>>
        {
            #region Private / Protected Fields

            private static MetadataViewProvider_Default_Type s_instance;
            private static Exception s_initialException = null;

            #endregion // Private / Protected Fields

            #region Constructor

            #region Documentation
            /// <summary>
            /// Constructor
            /// </summary>
            #endregion
            private MetadataViewProvider_Default_Type()
            {
            }

            #region Static Constructor

            #region Documentation
            /// <summary>
            /// This class follow the singleton pattern
            /// </summary>
            #endregion
            static MetadataViewProvider_Default_Type()
            {
                try
                {
                    s_instance = new MetadataViewProvider_Default_Type();
                }
                #region Exception Handling
                catch (SecurityException exc)
                {
                    s_initialException = exc;
                }
                catch (Exception exc)
                {
                    s_initialException = exc;
                }
                #endregion // Exception Handling
            }

            #endregion // Static Constructor

            #endregion // Constructor

            #region Public Static Propertis

            #region Instance

            #region Documentation
            /// <summary>
            /// Return the singleton instance.
            /// </summary>
            #endregion
            public static MetadataViewProvider_Default_Type Instance
            {
                get
                {
                    if (s_initialException != null)
                        throw s_initialException;

                    return s_instance;
                }
            }

            #endregion // Instance

            #endregion // Public Static Propertis

            #region IMetadataViewProvider<IMetadataView> Members

            public Dictionary<string, object> GetMetadataView(IDictionary<string, object> metadata)
            {
                if (metadata is Dictionary<string, object>)
                    return (Dictionary<string, object>)metadata;
                var dic = new Dictionary<string, object>(metadata);
                return dic;
            }

            #endregion
        }

        #endregion // MetadataViewProvider_Default_Type

        #region MetadataViewProvider_Default_IEnumerable

        #region Documentation
        /// <summary>
        /// Singleton implementation of IMetadataViewProvider<IDictionary<string,object>>
        /// </summary>
        #endregion // Documentation
        private class MetadataViewProvider_Default_IEnumerable : IMetadataViewProvider<IEnumerable<KeyValuePair<string, object>>>
        {
            #region Private / Protected Fields

            private static MetadataViewProvider_Default_IEnumerable s_instance;
            private static Exception s_initialException = null;

            #endregion // Private / Protected Fields

            #region Constructor

            #region Documentation
            /// <summary>
            /// Constructor
            /// </summary>
            #endregion
            private MetadataViewProvider_Default_IEnumerable()
            {
            }

            #region Static Constructor

            #region Documentation
            /// <summary>
            /// This class follow the singleton pattern
            /// </summary>
            #endregion
            static MetadataViewProvider_Default_IEnumerable()
            {
                try
                {
                    s_instance = new MetadataViewProvider_Default_IEnumerable();
                }
                #region Exception Handling
                catch (SecurityException exc)
                {
                    s_initialException = exc;
                }
                catch (Exception exc)
                {
                    s_initialException = exc;
                }
                #endregion // Exception Handling
            }

            #endregion // Static Constructor

            #endregion // Constructor

            #region Public Static Propertis

            #region Instance

            #region Documentation
            /// <summary>
            /// Return the singleton instance.
            /// </summary>
            #endregion
            public static MetadataViewProvider_Default_IEnumerable Instance
            {
                get
                {
                    if (s_initialException != null)
                        throw s_initialException;

                    return s_instance;
                }
            }

            #endregion // Instance

            #endregion // Public Static Propertis

            #region IMetadataViewProvider<IMetadataView> Members

            public IEnumerable<KeyValuePair<string, object>> GetMetadataView(IDictionary<string, object> metadata)
            {
                return metadata;
            }

            #endregion
        }

        #endregion // MetadataViewProvider_Default_IEnumerable

		#endregion // Nested Types
    }
}
