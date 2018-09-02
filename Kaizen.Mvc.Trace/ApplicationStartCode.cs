using Kaizen.Mvc.Trace;
using System.Configuration;
using System.Web;
using System.Web.Mvc;

[assembly: PreApplicationStartMethod(typeof(ApplicationStartCode), "Start")]
namespace Kaizen.Mvc.Trace
{
    public class ApplicationStartCode
    {
        static bool _disableKaizenMvcTrace = false;
        internal static bool DisableKaizenMvcTrace {
            get {
                return _disableKaizenMvcTrace;
            }
        }
        
        public static void Start()
        {

            //Microsoft.Web.Infrastructure.DynamicModuleHelper.DynamicModuleUtility.RegisterModule(typeof(WatchModule)); // Register our module
            //HttpApplication.RegisterModule(typeof(WatchModule)); // Register our module
            var value = ConfigurationManager.AppSettings["DisableKaizenMvcTrace"];
            if (value != null )
                bool.TryParse(value, out _disableKaizenMvcTrace);
            if(!_disableKaizenMvcTrace)
                GlobalFilters.Filters.Add(new ActionFilter()); //Register ActionFilter

        }
    }
}
