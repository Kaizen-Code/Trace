using Kaizen.Performance.Service;
using Kaizen.Performance.Service.Interface;
using Performance.Model;
using System.Web.Mvc;

namespace Kaizen.Mvc.Trace
{
    public class ActionFilter : /*ActionFilterAttribute,*/ IActionFilter
    {
        public static bool IsDisabled
        {
            get
            {
                return _disabledWatch;
            }
            set {
                _disabledWatch = value;
            }
        }
        static bool _disabledWatch = false;
        
        //public override void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //    base.OnActionExecuting(filterContext);
        //}

        //public override void OnActionExecuted(ActionExecutedContext filterContext)
        //{
        //    base.OnActionExecuted(filterContext);
        //}


        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.IsChildAction)
                return;
            if (_disabledWatch)
                return;
            IWatchService iWatch = ServiceFactory.WatchService;
            iWatch.Start();
            filterContext.HttpContext.Items.Add(Constants.WatchItemName, iWatch);

        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.IsChildAction)
                return;
            var service = filterContext.HttpContext.Items[Constants.WatchItemName];
            if (service == null)
                return; // do nothing.
            var area = filterContext.RouteData.DataTokens["area"];
            var controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            var action = filterContext.ActionDescriptor.ActionName;
            ((IWatchService)service).Stop($"{area}/{controller}/{action}");

        }
    }



}
