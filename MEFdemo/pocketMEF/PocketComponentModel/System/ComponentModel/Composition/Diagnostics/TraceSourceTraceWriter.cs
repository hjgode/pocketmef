// -----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
// -----------------------------------------------------------------------
#if !SILVERLIGHT

using System;
using System.Diagnostics;
using Microsoft.Internal;
using System.Text;

namespace System.ComponentModel.Composition.Diagnostics
{
    // Represents a trace writer that writes to a System.Diagnostics TraceSource
    internal sealed class TraceSourceTraceWriter : TraceWriter
    {        
        public override bool CanWriteInformation
        {
            get { return false; }
        }

        public override bool CanWriteWarning
        {
            get { return false; }
        }

        public override bool CanWriteError
        {
            get { return false; }
        }

        public override void WriteInformation(CompositionTraceId traceId, string format, params object[] arguments)
        {
            WriteEvent(TraceEventType.Information, traceId, format, arguments);
        }

        public override void WriteWarning(CompositionTraceId traceId, string format, params object[] arguments)
        {
            WriteEvent(TraceEventType.Warning, traceId, format, arguments);
        }

        public override void WriteError(CompositionTraceId traceId, string format, params object[] arguments)
        {
            WriteEvent(TraceEventType.Error, traceId, format, arguments);
        }

        private static void WriteEvent(TraceEventType eventType, CompositionTraceId traceId, string format, params object[] arguments)
        {
            const string LEVEL = "Level = ";
            const string ID = " ,Id = ";
            const string NEW_LINE = "\r\n";
            StringBuilder sb = new StringBuilder (60);
            sb.Append (LEVEL);
            sb.Append (eventType);
            sb.Append (ID);
            sb.Append (traceId);
            sb.Append (NEW_LINE);
            sb.AppendFormat (format, arguments);
            var mssage = eventType.ToString() + string.Format(format, arguments);
        }

        private enum TraceEventType
        {
            Information,
            Warning,
            Error
        }
    }
}

#endif