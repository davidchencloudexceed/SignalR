// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Threading;



namespace CommonUtility.Infrastructure
{
    /// <summary>
    /// Manages performance counters using Windows performance counters.
    /// </summary>
    public class PerformanceCounterManager : IPerformanceCounterManager
    {
        /// <summary>
        /// The performance counter category name for SignalR counters.
        /// </summary>
        public const string CategoryName = "TBQuoteServer";

        private readonly static PropertyInfo[] _counterProperties = GetCounterPropertyInfo();
        private readonly static IPerformanceCounter _noOpCounter = new NoOpPerformanceCounter();
        private volatile bool _initialized;
        private object _initLocker = new object();

        public PerformanceCounterManager()
        {
            InitNoOpCounters();
        }
        
        /// <summary>
        /// Gets the performance counter representing the toal number of messages received by connections (server to client) since the application was started.
        /// </summary>
        [PerformanceCounter(Name = "Messages Received From Receiver Total", Description = "The toal number of messages received by connections (server to client) since the application was started.", CounterType = PerformanceCounterType.NumberOfItems64)]
        public IPerformanceCounter MessagesReceivedFromReceiverTotal { get; private set; }

        /// <summary>
        /// Gets the performance counter representing the total number of messages sent by connections (client to server) since the application was started.
        /// </summary>
        [PerformanceCounter(Name = "Messages Sent To Clients Total", Description = "The total number of messages sent by connections (client to server) since the application was started.", CounterType = PerformanceCounterType.NumberOfItems64)]
        public IPerformanceCounter MessagesSentToClientsTotal { get; private set; }

        /// <summary>
        /// Gets the performance counter representing the number of messages received by connections (server to client) per second.
        /// </summary>
        [PerformanceCounter(Name = "Messages Received From Receiver/Sec", Description = "The number of messages received by connections (server to client) per second.", CounterType = PerformanceCounterType.RateOfCountsPerSecond32)]
        public IPerformanceCounter MessagesReceivedFromReceiverPerSec { get; private set; }

        /// <summary>
        /// Gets the performance counter representing the number of messages sent by connections (client to server) per second.
        /// </summary>
        [PerformanceCounter(Name = "Messages Sent To Clients/Sec", Description = "The number of messages sent by connections (client to server) per second.", CounterType = PerformanceCounterType.RateOfCountsPerSecond32)]
        public IPerformanceCounter MessagesSentToClientsPerSec { get; private set; }

        
        internal string InstanceName { get; private set; }

        /// <summary>
        /// Initializes the performance counters.
        /// </summary>
        /// <param name="instanceName">The host instance name.</param>
        /// <param name="hostShutdownToken">The CancellationToken representing the host shutdown.</param>
        public void Initialize(string instanceName, CancellationToken hostShutdownToken)
        {
            if (_initialized)
            {
                return;
            }

            var needToRegisterWithShutdownToken = false;
            lock (_initLocker)
            {
                if (!_initialized)
                {
                    InstanceName = SanitizeInstanceName(instanceName);
                    SetCounterProperties();
                    // The initializer ran, so let's register the shutdown cleanup
                    if (hostShutdownToken != CancellationToken.None)
                    {
                        needToRegisterWithShutdownToken = true;
                    }
                    _initialized = true;
                }
            }

            if (needToRegisterWithShutdownToken)
            {
                hostShutdownToken.Register(UnloadCounters);
            }
        }

        private void UnloadCounters()
        {
            lock (_initLocker)
            {
                if (!_initialized)
                {
                    // We were never initalized
                    return;
                }
            }

            var counterProperties = this.GetType()
                .GetProperties()
                .Where(p => p.PropertyType == typeof(IPerformanceCounter));

            foreach (var property in counterProperties)
            {
                var counter = property.GetValue(this, null) as IPerformanceCounter;
                counter.Close();
                counter.RemoveInstance();
            }
        }

        private void InitNoOpCounters()
        {
            // Set all the counter properties to no-op by default.
            // These will get reset to real counters when/if the Initialize method is called.
            foreach (var property in _counterProperties)
            {
                property.SetValue(this, new NoOpPerformanceCounter(), null);
            }
        }

        private void SetCounterProperties()
        {
            var loadCounters = true;

            foreach (var property in _counterProperties)
            {
                PerformanceCounterAttribute attribute = GetPerformanceCounterAttribute(property);

                if (attribute == null)
                {
                    continue;
                }

                IPerformanceCounter counter = null;

                if (loadCounters)
                {
                    counter = LoadCounter(CategoryName, attribute.Name, isReadOnly:false);

                    if (counter == null)
                    {
                        // We failed to load the counter so skip the rest
                        loadCounters = false;
                    }
                }

                counter = counter ?? _noOpCounter;

                property.SetValue(this, counter, null);
            }
        }

        public static PropertyInfo[] GetCounterPropertyInfo()
        {
            return typeof(PerformanceCounterManager)
                .GetProperties()
                .Where(p => p.PropertyType == typeof(IPerformanceCounter))
                .ToArray();
        }

        public static PerformanceCounterAttribute GetPerformanceCounterAttribute(PropertyInfo property)
        {
            return property.GetCustomAttributes(typeof(PerformanceCounterAttribute), false)
                    .Cast<PerformanceCounterAttribute>()
                    .SingleOrDefault();
        }

        private static string SanitizeInstanceName(string instanceName)
        {
            // Details on how to sanitize instance names are at http://msdn.microsoft.com/en-us/library/vstudio/system.diagnostics.performancecounter.instancename
            if (string.IsNullOrWhiteSpace(instanceName))
            {
                instanceName = Guid.NewGuid().ToString();
            }
            
            // Substitute invalid chars with valid replacements
            var substMap = new Dictionary<char, char> {
                { '(', '[' },
                { ')', ']' },
                { '#', '-' },
                { '\\', '-' },
                { '/', '-' }
            };
            var sanitizedName = new String(instanceName.Select(c => substMap.ContainsKey(c) ? substMap[c] : c).ToArray());

            // Names must be shorter than 128 chars, see doc link above
            var maxLength = 127;
            return sanitizedName.Length <= maxLength
                ? sanitizedName
                : sanitizedName.Substring(0, maxLength);
        }

        private IPerformanceCounter LoadCounter(string categoryName, string counterName, bool isReadOnly)
        {
            return LoadCounter(categoryName, counterName, InstanceName, isReadOnly);
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "This file is shared")]
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Counters are disposed later")]
        public IPerformanceCounter LoadCounter(string categoryName, string counterName, string instanceName, bool isReadOnly)
        {
            var counter = new PerformanceCounter(categoryName, counterName, instanceName, isReadOnly);

            // Initialize the counter sample
            counter.NextSample();

            return new PerformanceCounterWrapper(counter);
        }
    }
}
