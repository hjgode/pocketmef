﻿// -----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
// -----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Internal;
using System.Globalization;

namespace System.ComponentModel.Composition.Primitives
{
    /// <summary>
    ///     Represents an import required by a <see cref="ComposablePart"/> object.
    /// </summary>
    public class ImportDefinition
    {
        internal static readonly string EmptyContractName = string.Empty;
        private readonly Func<ExportDefinition, bool> _constraint;
        private readonly ImportCardinality _cardinality = ImportCardinality.ExactlyOne;
        private readonly string _contractName = EmptyContractName;
        private readonly bool _isRecomposable;
        private readonly bool _isPrerequisite = true;
        private Func<ExportDefinition, bool> _compiledConstraint;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ImportDefinition"/> class.
        /// </summary>
        /// <remarks>
        ///     <note type="inheritinfo">
        ///         Derived types calling this constructor must override the <see cref="Constraint"/> 
        ///         property, and optionally, the <see cref="Cardinality"/>, <see cref="IsPrerequisite"/> 
        ///         and <see cref="IsRecomposable"/> 
        ///         properties.
        ///     </note>
        /// </remarks>
        protected ImportDefinition()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ImportDefinition"/> class 
        ///     with the specified constraint, cardinality, value indicating if the import 
        ///     definition is recomposable and a value indicating if the import definition 
        ///     is a prerequisite.
        /// </summary>
        /// <param name="constraint">
        ///     A <see cref="Expression{TDelegate}"/> containing a <see cref="Func{T, TResult}"/> 
        ///     that defines the conditions that must be matched for the <see cref="ImportDefinition"/> 
        ///     to be satisfied by an <see cref="Export"/>.
        /// </param>
        /// <param name="contractName">
        ///     The contract name of the export that this import is interested in. The contract name
        ///     property is used as guidance and not automatically enforced in the constraint. If 
        ///     the contract name is a required in the constraint then it should be added to the constraint
        ///     by the caller of this constructor.
        /// </param>
        /// <param name="cardinality">
        ///     One of the <see cref="ImportCardinality"/> values indicating the 
        ///     cardinality of the <see cref="Export"/> objects required by the
        ///     <see cref="ImportDefinition"/>.
        /// </param>
        /// <param name="isRecomposable">
        ///     <see langword="true"/> if the <see cref="ImportDefinition"/> can be satisfied 
        ///     multiple times throughout the lifetime of a <see cref="ComposablePart"/>, otherwise, 
        ///     <see langword="false"/>.
        /// </param>
        /// <param name="isPrerequisite">
        ///     <see langword="true"/> if the <see cref="ImportDefinition"/> is required to be 
        ///     satisfied before a <see cref="ComposablePart"/> can start producing exported 
        ///     objects; otherwise, <see langword="false"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="constraint"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="cardinality"/> is not one of the <see cref="ImportCardinality"/> 
        ///     values.
        /// </exception>
        public ImportDefinition(Func<ExportDefinition, bool> constraint, string contractName, ImportCardinality cardinality, bool isRecomposable, bool isPrerequisite)
            : this(contractName, cardinality, isRecomposable, isPrerequisite)
        {
            Requires.NotNull(constraint, "constraint");

            this._constraint = constraint;
        }

        internal ImportDefinition(string contractName, ImportCardinality cardinality, bool isRecomposable, bool isPrerequisite)
        {
            if (
                (cardinality != ImportCardinality.ExactlyOne) &&
                (cardinality != ImportCardinality.ZeroOrMore) &&
                (cardinality != ImportCardinality.ZeroOrOne)
                )
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.ArgumentOutOfRange_InvalidEnum, "cardinality", cardinality, typeof(ImportCardinality).Name), "cardinality");
            }

            this._contractName = contractName ?? EmptyContractName;
            this._cardinality = cardinality;
            this._isRecomposable = isRecomposable;
            this._isPrerequisite = isPrerequisite;
        }

        /// <summary>
        ///     Gets the contract name of the export required by the import definition.
        /// </summary>
        /// <value>
        ///     A <see cref="String"/> containing the contract name of the <see cref="Export"/> 
        ///     required by the <see cref="ContractBasedImportDefinition"/>. This property should
        ///     return <see cref="String.Empty"/> for imports that do not require a specific 
        ///     contract name.
        /// </value>
        public virtual string ContractName
        {
            get { return this._contractName; }
        }

        /// <summary>
        ///     Gets the cardinality of the exports required by the import definition.
        /// </summary>
        /// <value>
        ///     One of the <see cref="ImportCardinality"/> values indicating the 
        ///     cardinality of the <see cref="Export"/> objects required by the
        ///     <see cref="ImportDefinition"/>. The default is 
        ///     <see cref="ImportCardinality.ExactlyOne"/>
        /// </value>
        public virtual ImportCardinality Cardinality
        {
            get { return this._cardinality; }
        }

        /// <summary>
        ///     Gets an expression that defines conditions that must be matched for the import 
        ///     described by the import definition to be satisfied.
        /// </summary>
        /// <returns>
        ///     A <see cref="Expression{TDelegate}"/> containing a <see cref="Func{T, TResult}"/> 
        ///     that defines the conditions that must be matched for the 
        ///     <see cref="ImportDefinition"/> to be satisfied by an <see cref="Export"/>.
        /// </returns>
        /// <exception cref="NotImplementedException">
        ///     The property was not overridden by a derived class.
        /// </exception>
        /// <remarks>
        ///     <note type="inheritinfo">
        ///         Overriders of this property should never return <see langword="null"/>.
        ///     </note>
        /// </remarks>
        public virtual Func<ExportDefinition, bool> Constraint
        {
            get
            {
                if (this._constraint != null)
                {
                    return this._constraint;
                }

                throw ExceptionBuilder.CreateNotOverriddenByDerived("Constraint");
            }
        }

        /// <summary>
        ///     Gets a value indicating whether the import definition is required to be 
        ///     satisfied before a part can start producing exported values.
        /// </summary>
        /// <value>
        ///     <see langword="true"/> if the <see cref="ImportDefinition"/> is required to be 
        ///     satisfied before a <see cref="ComposablePart"/> can start producing exported 
        ///     objects; otherwise, <see langword="false"/>. The default is <see langword="true"/>.
        /// </value>
        public virtual bool IsPrerequisite
        {
            get { return this._isPrerequisite; }
        }

        /// <summary>
        ///     Gets a value indicating whether the import definition can be satisfied multiple times.
        /// </summary>
        /// <value>
        ///     <see langword="true"/> if the <see cref="ImportDefinition"/> can be satisfied 
        ///     multiple times throughout the lifetime of a <see cref="ComposablePart"/>, otherwise, 
        ///     <see langword="false"/>. The default is <see langword="false"/>.
        /// </value>
        public virtual bool IsRecomposable
        {
            get { return this._isRecomposable; }
        }

        /// <summary>
        ///     Executes of the constraint provided by the <see cref="Constraint"/> property
        ///     against a given <see cref="ExportDefinition"/> to determine if this 
        ///     <see cref="ImportDefinition"/> can be satisfied by the given <see cref="Export"/>.
        /// </summary>
        /// <param name="exportDefinition">
        ///     A definition for a <see cref="Export"/> used to determine if it satisfies the
        ///     requirements for this <see cref="ImportDefinition"/>.
        /// </param>
        /// <returns>
        ///     <see langword="True"/> if the <see cref="Export"/> satisfies the requirements for
        ///     this <see cref="ImportDefinition"/>, otherwise returns <see langword="False"/>.
        /// </returns>
        /// <remarks>
        ///     <note type="inheritinfo">
        ///         Overrides of this method can provide a more optimized execution of the 
        ///         <see cref="Constraint"/> property but the result should remain consistent.
        ///     </note>
        /// </remarks>
        public virtual bool IsConstraintSatisfiedBy(ExportDefinition exportDefinition)
        {
            if (this._compiledConstraint == null)
            {
                this._compiledConstraint = this.Constraint;
            }

            return this._compiledConstraint.Invoke(exportDefinition);
        }

        /// <summary>
        ///     Returns a string representation of the import definition.
        /// </summary>
        /// <returns>
        ///     A <see cref="String"/> containing the value of the <see cref="Constraint"/> property.
        /// </returns>
        public override string ToString()
        {
            return this.Constraint.ToString();
        }
    }
}
