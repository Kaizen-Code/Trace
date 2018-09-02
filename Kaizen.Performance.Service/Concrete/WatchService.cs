using Kaizen.Performance.Service.Interface;
using Performance.Model;
using Performance.Store;
using Performance.Store.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Kaizen.Performance.Service.Concrete
{
    class WatchService : IWatchService
    {
        static IWatchLogStore iStore;
        static WatchService()
        {
            iStore = StoreFactory.WatchLogStore;
        }
        Stopwatch stopwatch;

        public void Start()
        {
            stopwatch = new Stopwatch();
            stopwatch.Start();
        }

        public void Stop(string requestUrl)
        {
            if (stopwatch != null)
            {
                stopwatch.Stop();
                var model = new WatchLog()
                {
                    Url = requestUrl,
                    Milliseconds = stopwatch.ElapsedMilliseconds,
                    Ticks = stopwatch.ElapsedTicks
                };
                iStore.Add(model);
                stopwatch = null;
            }
        }

        public IEnumerable<object> GetLogReport()
        {
            var result = iStore.FindAll().GroupBy(c => c.Url)
                .Select(group => new {
                    Url = group.Key,

                    Count = group.Count(),

                    AvgMilliseconds = Math.Round(group.Average(c => c.Milliseconds), 2),
                    MinMilliseconds = group.Min(c => c.Milliseconds),
                    MaxMilliseconds = group.Max(c => c.Milliseconds),

                    AvgTicks = Math.Round(group.Average(c => c.Ticks), 2),
                    MinTicks = group.Min(c => c.Ticks),
                    MaxTicks = group.Max(c => c.Ticks)

                }).OrderByDescending(c => c.AvgTicks);
            return result;
        }

        public IEnumerable<ConsolidateLog> GetConsolidateLogs(ReportType reportType)
        {
            var grouped = iStore.FindAll().GroupBy(c => c.Url);
            IEnumerable<ConsolidateLog> result;
            switch (reportType)
            {
                case ReportType.Ticks:
                    result = grouped.Select(group => new ConsolidateLog
                    {
                        Url = group.Key,
                        Count = group.Count(),
                        Avg = Math.Round(group.Average(c => c.Ticks), 2),
                        Min = group.Min(c => c.Ticks),
                        Max = group.Max(c => c.Ticks)
                    });
                    break;
                case ReportType.Milliseconds:
                default:
                    result = grouped.Select(group => new ConsolidateLog
                    {
                        Url = group.Key,
                        Count = group.Count(),
                        Avg = Math.Round(group.Average(c => c.Milliseconds), 2),
                        Min = group.Min(c => c.Milliseconds),
                        Max = group.Max(c => c.Milliseconds)
                    });
                    break;
            }
            return result;
        }
    }

}
