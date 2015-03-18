// -----------------------------------------------------------------------
// Copyright (c) Bnaya Eshet.  All rights reserved.
// -----------------------------------------------------------------------
#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Reflection;
using Microsoft.Internal;

#endregion // Using

namespace System.ComponentModel.Composition
{
    #region Documentation
    /// <summary>
    /// Use as constraint for specific (Import) state
    /// </summary>
    /// <remarks>
    /// It is kid of cache for the import constraint
    /// </remarks>
    #endregion // Documentation
    internal class ConstraintInstance
    {
        #region Private / Protected Fields

        private readonly IEnumerable<KeyValuePair<string, Type>> _requiredMetadata;
        private readonly string _contractName;
        private readonly string _requiredTypeIdentity;
        private readonly CreationPolicy _requiredCreationPolicy;

        #endregion // Private / Protected Fields

        #region Constructors

        #region Documentation
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="requiredMetadata"></param>
        #endregion // Documentation
        public ConstraintInstance(IEnumerable<KeyValuePair<string, Type>> requiredMetadata)
        {
            _requiredMetadata = requiredMetadata;
        }

        #region Documentation
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="contractName"></param>
        /// <param name="requiredTypeIdentity"></param>
        /// <param name="requiredMetadata"></param>
        /// <param name="requiredCreationPolicy"></param>
        #endregion // Documentation
        public ConstraintInstance(
            string contractName,
            string requiredTypeIdentity,
            IEnumerable<KeyValuePair<string, Type>> requiredMetadata,
            CreationPolicy requiredCreationPolicy): this (requiredMetadata)
        {
            _contractName = contractName;
            _requiredTypeIdentity = requiredTypeIdentity;
            _requiredCreationPolicy = requiredCreationPolicy;
        }

        #endregion // Constructors

        #region Private Methods

        #region Documentation
        /// <summary>
        /// The actual constraint
        /// </summary>
        /// <param name="expDef"></param>
        /// <returns></returns>
        #endregion // Documentation
        private bool ExportDefinitionConstraint(ExportDefinition expDef)
        {
            if (expDef == null)
                return false;
            if (_contractName != null && expDef.ContractName != _contractName)
                return false;
            if (expDef.Metadata.ContainsKey(CompositionConstants.ExportTypeIdentityMetadataName) &&
                expDef.Metadata[CompositionConstants.ExportTypeIdentityMetadataName] as string != _requiredTypeIdentity)
            {
                return false;
            }
            // TODO: _requiredCreationPolicy

            if (_requiredMetadata != null)
            {
                foreach (KeyValuePair<string, Type> pair in _requiredMetadata)
                {
                    if (!expDef.Metadata.ContainsKey(pair.Key))
                        return false;
                }
            }

            return true;
        }

        #endregion // Private Methods

        #region Public Methods

        #region Documentation
        /// <summary>
        /// Return constraint function
        /// </summary>
        /// <returns></returns>
        #endregion // Documentation
        public Func<ExportDefinition, bool> GetExportDefinitionConstraint()
        {
            return ExportDefinitionConstraint;
        }

        #endregion // Public Methods

    }
}