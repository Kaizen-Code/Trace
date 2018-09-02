using Performance.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Performance.Store.Interface
{
    public interface IWatchLogStore
    {
        int Add(WatchLog obj);

        IEnumerable<WatchLog> FindAll();
    }
}
