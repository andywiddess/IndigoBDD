#region Header
// ---------------------------------------------------------------------------
// Sepura - Commercially Confidential.
// 
// Indigo.CrossCutting.Utilities.LockProvider
// Lock Strategy Provider
//
// Copyright (c) 2016 Sepura Plc
// All Rights reserved.
//
// $Id:  $ :
// ---------------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Indigo.CrossCutting.Utilities
{
    /// <summary>
    /// Generic Lock Provider we are using for the Cpd Services
    /// </summary>
    public static class LockProvider
    {
        #region Static Methods
        /// <summary>
        /// Opens the specified reader writer lock in read mode,
        /// specifying whether or not it may be upgraded.
        /// </summary>
        /// <param name="slim"></param>
        /// <param name="upgradeable"></param>
        /// <returns></returns>
        public static IDisposable Read(this ReaderWriterLockSlim slim, bool upgradeable)
        {
            return new ReaderWriterLockSlimController(slim, true, upgradeable);
        }

        /// <summary>
        /// Opens the specified reader writer lock in read mode,
        /// and does not allow upgrading.
        /// </summary>
        /// <param name="slim"></param>
        /// <returns></returns>
        public static IDisposable Read(this ReaderWriterLockSlim slim)
        {
            return new ReaderWriterLockSlimController(slim, true, false);
        }

        /// <summary>
        /// Opens the specified reader writer lock in write mode.
        /// </summary>
        /// <param name="slim"></param>
        /// <returns></returns>
        public static IDisposable Write(this ReaderWriterLockSlim slim)
        {
            return new ReaderWriterLockSlimController(slim, false, false);
        }
        #endregion

        #region Private Class
        /// <summary>
        /// Implementation of the RWLS controller to support a locking strategy for the aggregation service
        /// </summary>
        private class ReaderWriterLockSlimController 
            : IDisposable
        {
            #region Members
            /// <summary>
            /// Have we closed and disposed of the lock strategy
            /// </summary>
            private bool closed = false;

            /// <summary>
            /// Are we in read or write mode
            /// </summary>
            private bool read = false;

            /// <summary>
            /// RWLS instance
            /// </summary>
            private ReaderWriterLockSlim slim;

            /// <summary>
            /// Do we need to upgrade the locking strategy
            /// </summary>
            private bool upgrade = false;
            #endregion

            #region Constructors
            /// <summary>
            /// Initializes a new instance of the <see cref="ReaderWriterLockSlimController"/> class.
            /// </summary>
            /// <param name="slim">The slim.</param>
            /// <param name="read">if set to <c>true</c> [read].</param>
            /// <param name="upgrade">if set to <c>true</c> [upgrade].</param>
            public ReaderWriterLockSlimController(ReaderWriterLockSlim slim, bool read, bool upgrade)
            {
                this.slim = slim;
                this.read = read;
                this.upgrade = upgrade;

                if (read)
                {
                    if (upgrade)
                    {
                        slim.EnterUpgradeableReadLock();
                    }
                    else
                    {
                        slim.EnterReadLock();
                    }
                }
                else
                {
                    slim.EnterWriteLock();
                }
            }

            /// <summary>
            /// Finalizes an instance of the <see cref="ReaderWriterLockSlimController"/> class.
            /// </summary>
            ~ReaderWriterLockSlimController()
            {
                Dispose();
            }
            #endregion

            #region IDisposable Implementation
            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                if (closed)
                    return;

                closed = true;

                if (read)
                {
                    if (upgrade)
                    {
                        slim.ExitUpgradeableReadLock();
                    }
                    else
                    {
                        slim.ExitReadLock();
                    }
                }
                else
                {
                    slim.ExitWriteLock();
                }

                GC.SuppressFinalize(this);
            }
            #endregion 
        }
        #endregion
    }
}
