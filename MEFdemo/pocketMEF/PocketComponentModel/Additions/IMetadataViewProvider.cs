// -----------------------------------------------------------------------
// Copyright (c) Bnaya Eshet.  All rights reserved.
// -----------------------------------------------------------------------
#region Using

using System.Collections.Generic;

#endregion // Using

namespace System.ComponentModel.Composition
{
    #region Documentation
    /// <summary>
    /// Metadata view provider
    /// </summary>
    /// <remarks>
    /// Used for converting the metadata into concrete type
    /// </remarks>
    #endregion // Documentation
    internal interface IMetadataViewProvider<TMetadataView>
    {
        TMetadataView GetMetadataView(IDictionary<string, object> metadata);
    }
}