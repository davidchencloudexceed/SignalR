using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonUtility.Infrastructure;

namespace Microsoft.AspNet.SelfHost.Samples
{
    internal class PerformanceCounterInstaller
    {
        public IList<string> InstallCounters()
        {
            // Delete any existing counters
            UninstallCounters();

            var counterCreationData = CommonUtility.Infrastructure.PerformanceCounterManager.GetCounterPropertyInfo()
                .Select(p =>
                {
                    var attribute = CommonUtility.Infrastructure.PerformanceCounterManager.GetPerformanceCounterAttribute(p);
                    return new CounterCreationData(attribute.Name, attribute.Description, attribute.CounterType);
                })
                .ToArray();

            var createDataCollection = new CounterCreationDataCollection(counterCreationData);

            PerformanceCounterCategory.Create(CommonUtility.Infrastructure.PerformanceCounterManager.CategoryName,
                "SignalR application performance counters",
                PerformanceCounterCategoryType.MultiInstance,
                createDataCollection);

            return counterCreationData.Select(c => c.CounterName).ToList();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Called from non-static.")]
        public void UninstallCounters()
        {
            if (PerformanceCounterCategory.Exists(CommonUtility.Infrastructure.PerformanceCounterManager.CategoryName))
            {
                PerformanceCounterCategory.Delete(CommonUtility.Infrastructure.PerformanceCounterManager.CategoryName);
            }
        }
    }
}
