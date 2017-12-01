// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;

namespace CommonUtility.Infrastructure
{
    /// <summary>
    /// Provides access to performance counters.
    /// </summary>
    public interface IPerformanceCounterManager
    {
        /// <summary>
        /// Initializes the performance counters.
        /// </summary>
        /// <param name="instanceName">The host instance name.</param>
        /// <param name="hostShutdownToken">The CancellationToken representing the host shutdown.</param>
        void Initialize(string instanceName, CancellationToken hostShutdownToken);

        /// <summary>
        /// Loads a performance counter.
        /// </summary>
        /// <param name="categoryName">The category name.</param>
        /// <param name="counterName">The counter name.</param>
        /// <param name="instanceName">The instance name.</param>
        /// <param name="isReadOnly">Whether the counter is read-only.</param>
        IPerformanceCounter LoadCounter(string categoryName, string counterName, string instanceName, bool isReadOnly);

        /// <summary>
        /// Gets the performance counter representing the total number of messages received by connections (server to client) since the application was started.
        /// </summary>
        IPerformanceCounter MessagesReceivedFromReceiverTotal { get; }

        /// <summary>
        /// Gets the performance counter representing the total number of messages received by connections (server to client) since the application was started.
        /// </summary>
        IPerformanceCounter MessagesSentToClientsTotal { get; }

        /// <summary>
        /// Gets the performance counter representing the number of messages received by connections (server to client) per second.
        /// </summary>
        IPerformanceCounter MessagesReceivedFromReceiverPerSec { get; }

        /// <summary>
        /// Gets the performance counter representing the number of messages sent by connections (client to server) per second.
        /// </summary>
        IPerformanceCounter MessagesSentToClientsPerSec { get; }

        
    }
}
