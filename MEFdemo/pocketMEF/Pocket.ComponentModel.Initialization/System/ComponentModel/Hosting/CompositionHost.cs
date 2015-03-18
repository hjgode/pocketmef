#region Using

using System;
using System.Globalization;
using System.Threading;
using System.Linq;
using System.ComponentModel.Composition.Primitives;
using System.IO;

#endregion // Using

namespace System.ComponentModel.Composition.Hosting
{
    public static class CompositionHost
    {
        #region Private / Protected Fields

        // Field is internal only to assist in testing
        internal static CompositionContainer _container = null;
        private static object _lockObject = new object();

#if(!PocketPC && !WindowsCE) 
        internal static string _PartsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Parts");
#else
        internal static string _PartsDirectory = Path.Combine(IOHelper.BaseDirectory, "Parts");
    #endif

        #endregion // Private / Protected Fields

        #region Public Properties

        #region Provider

        #region Documentation
        /// <summary>
        /// Gets the underline export provider
        /// </summary>
        #endregion // Documentation
        public static ExportProvider Provider { get { return _container; } }

        #endregion // Provider

        #endregion // Public Properties

        #region Public Methods

        #region Set Parts Directory

        #region Documentation
        /// <summary>
        /// Set the part directory 
        /// </summary>
        /// <remarks>
        /// Throw exception if the container is active
        /// </remarks>
        /// <param name="directory"></param>
        #endregion // Documentation
        public static void SetPartsDirectory(string directory)
        {
            if (_container != null)
            {
                throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture,
                    Strings.InvalidOperationException_GlobalContainerAlreadyInitialized));
            }
            _PartsDirectory = directory;
        }

        #endregion // Set Parts Directory

        #region Reset

        #region Documentation
        /// <summary>
        /// Reset the container
        /// </summary>
        #endregion // Documentation
        public static void Reset()
        {
            _container = null;
        }

        #endregion // Reset

        #region Initialize Container

        #region Documentation
        /// <summary>
        ///     This method can be used to initialize the global container used by <see cref="PartInitializer.SatisfyImports"/>
        ///     in case where the default container doesn't provide enough flexibility. 
        ///     
        ///     If this method is needed it should be called exactly once and as early as possible in the application host. It will need
        ///     to be called before the first call to <see cref="PartInitializer.SatisfyImports"/>
        /// </summary>
        /// <param name="container">
        ///     <see cref="CompositionContainer"/> that should be used instead of the default global container.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="container"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     Either <see cref="InitializeContainer" /> has already been called or someone has already made use of the global 
        ///     container via <see cref="PartInitializer.SatisfyImports"/>. In either case you need to ensure that it 
        ///     is called only once and that it is called early in the application host startup code.
        /// </exception>
        #endregion // Documentation
        public static void InitializeContainer(CompositionContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            CompositionContainer globalContainer = null;
            bool alreadyCreated = TryGetOrCreateContainer(() => container, out globalContainer);

            if (alreadyCreated)
            {
                throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture, 
                    Strings.InvalidOperationException_GlobalContainerAlreadyInitialized));
            }
        }

        #endregion // Initialize Container

        #region Compose Parts

        #region Documentation
        /// <summary>
        /// Add export instances
        /// </summary>
        /// <param name="attributedParts">instances that has export attributes</param>
        #endregion // Documentation
        public static void ComposeParts(params object[] attributedParts)
        {
            #region Validation

            if (_container == null)
                throw new NullReferenceException("container does not initialized");
            if (attributedParts == null || attributedParts.Length == 0)
                return;

            #endregion // Validation

            CompositionBatch batch = new CompositionBatch(
                attributedParts.Select(attributedPart => AttributedModelServices.CreatePart(attributedPart)).ToArray(),
                Enumerable.Empty<ComposablePart>());

            _container.Compose(batch);
        }

        #endregion // Compose Parts

        #endregion // Public Methods

        #region Internal Methods

        #region Try Get Or Create Container

        internal static bool TryGetOrCreateContainer(Func<CompositionContainer> createContainer, out CompositionContainer globalContainer)
        {
            bool alreadyCreated = true;
            if (_container == null)
            {
                var container = createContainer.Invoke();
                lock (_lockObject)
                {
                    if (_container == null)
                    {
                        Thread.MemoryBarrier();
                        _container = container;
                        alreadyCreated = false;
                    }
                }
            }
            globalContainer = _container;
            return alreadyCreated;
        }

        #endregion // Try Get Or Create Container

        #endregion // Internal Methods
    }
}