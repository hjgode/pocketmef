﻿// -----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
// -----------------------------------------------------------------------
using System;
using Microsoft.Internal;

namespace System.ComponentModel.Composition.Primitives
{
    // As most objects that implement ICompositionElement (such as Export, ComposablePart, 
    // ComposablePartCatalog, etc) are not serializable, this class is used as a serializable 
    // placeholder for these types when ICompositionElement is used within serializable types,
    // such as CompositionException, CompositionIssue, etc.
    [Serializable]
    internal class SerializableCompositionElement : ICompositionElement
    {
        private readonly string _displayName;
        private readonly ICompositionElement _origin;

        public SerializableCompositionElement(string displayName, ICompositionElement origin)
        {
//#if !SILVERLIGHT
//            Assumes.IsTrue(origin == null); // || origin.GetType().IsSerializable);
//#endif
            this._displayName = displayName ?? string.Empty;
            this._origin = origin;
        }

        public string DisplayName
        {
            get { return this._displayName; }
        }

        public ICompositionElement Origin
        {
            get { return this._origin; }
        }

        public override string ToString()
        {
            return this.DisplayName;
        }

        public static ICompositionElement FromICompositionElement(ICompositionElement element)
        {
            if (element == null)
            {   // Null is always serializable   

                return null;
            }

            ICompositionElement origin = FromICompositionElement(element.Origin);

            // Otherwise, we need to create a serializable wrapper
            return new SerializableCompositionElement(element.DisplayName, origin);
        }
    }
}
