// -----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
// -----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Microsoft.Internal
{
    internal sealed class Lock : IDisposable
    {

        // ReaderWriterLockSlim is not yet implemented on SilverLight
        // Satisfies our requirements until it is implemented
        object _thisLock = new object();

        public Lock()
        {
        }

        public void EnterReadLock()
        {
            Monitor.Enter(this._thisLock);
        }

        public void EnterWriteLock()
        {
            Monitor.Enter(this._thisLock);
        }

        public void ExitReadLock()
        {
            Monitor.Exit(this._thisLock);
        }

        public void ExitWriteLock()
        {
            Monitor.Exit(this._thisLock);
        }

        public void Dispose()
        {
        }

    }
}
