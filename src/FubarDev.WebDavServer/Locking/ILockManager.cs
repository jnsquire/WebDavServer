﻿// <copyright file="ILockManager.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

namespace FubarDev.WebDavServer.Locking
{
    /// <summary>
    /// The interface for a lock manager
    /// </summary>
    public interface ILockManager
    {
        /// <summary>
        /// Gets called when a lock was added
        /// </summary>
        event EventHandler<LockEventArgs> LockAdded;

        /// <summary>
        /// Gets called when a lock was released
        /// </summary>
        event EventHandler<LockEventArgs> LockReleased;

        /// <summary>
        /// Tries to issue a lock
        /// </summary>
        /// <param name="l">The lock to issue</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns>Either the list of locks preventing issuing a lock or the active lock created</returns>
        [NotNull]
        [ItemNotNull]
        Task<LockResult> LockAsync(ILock l, CancellationToken cancellationToken);

        /// <summary>
        /// Releases a lock with the given state token
        /// </summary>
        /// <param name="stateToken">The state token of the lock to release</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns><see langword="true" /> when there was a lock to remove</returns>
        [NotNull]
        Task<bool> ReleaseAsync([NotNull] Uri stateToken, CancellationToken cancellationToken);

        /// <summary>
        /// Gets all active locks
        /// </summary>
        /// <remarks>
        /// Be aware that the locks could've been released in the mean time by a concurrent
        /// access or by the <see cref="LockCleanupTask"/>.
        /// </remarks>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns>Returns all active locks</returns>
        [NotNull]
        [ItemNotNull]
        Task<IEnumerable<IActiveLock>> GetLocksAsync(CancellationToken cancellationToken);
    }
}
