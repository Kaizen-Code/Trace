using Performance.Store.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Performance.Model;

namespace Performance.Store.Concrete
{
    class WatchLogStore : IWatchLogStore
    {
        static WatchLog[] list = new WatchLog[500];
        static int cnt;
        static readonly object locker = new object();
        
        
        public int Add(WatchLog model)
        {
            lock(locker)
            {
                if (cnt > list.Length - 1) 
                    cnt = 0; // first in first out 
                list[cnt++] = model;
            }
            return 1;
        }

        public IEnumerable<WatchLog> FindAll()
        {
            return list.Where(c=> c != null);
        }
    }
}
