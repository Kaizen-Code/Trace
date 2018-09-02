using Performance.Store.Concrete;
using Performance.Store.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Performance.Store
{
    public class StoreFactory
    {
        public static IWatchLogStore WatchLogStore
        {
            get
            {
                return new WatchLogStore();
            }
        }
    }
}
