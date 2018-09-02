using Performance.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaizen.Performance.Service.Interface
{
    public interface IWatchService
    {
        void Start();
        void Stop(string requestUrl);

        IEnumerable<object> GetLogReport();

        IEnumerable<ConsolidateLog> GetConsolidateLogs(ReportType reportType);

    }
}
