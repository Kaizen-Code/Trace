using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Performance.Model
{
    public class WatchLog
    {
        public string Url { get; set; }
        public long Milliseconds { get; set; }
        public long Ticks { get; set; }

    }
}
