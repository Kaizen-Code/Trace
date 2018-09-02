using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Performance.Model
{
    public class ConsolidateLog
    {
        public string Url { get; set; }
        public int Count { get; set; }

        public double Avg { get; set; }
        public long Max { get; set; }
        public long Min { get; set; }
    }
}
