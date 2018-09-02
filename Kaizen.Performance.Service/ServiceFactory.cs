using Kaizen.Performance.Service.Concrete;
using Kaizen.Performance.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaizen.Performance.Service
{
    public class ServiceFactory
    {
        public static IWatchService WatchService
        {
            get
            {
                return new WatchService();
            }
        }
    }
}
