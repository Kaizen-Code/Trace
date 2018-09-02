
using Kaizen.Performance.Service;
using Kaizen.Performance.Service.Interface;
using Performance.Model;
using System;
using System.Web;
using System.Web.Mvc;

namespace Kaizen.Mvc.Trace
{
    public class Module : IHttpModule
    {
        /// <summary>
        /// You will need to configure this module in the Web.config file of your
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpModule Members
            

        public void Dispose()
        {
            //clean-up code here.
        }

        public void Init(HttpApplication context)
        {
            // Below is an example of how you can handle LogRequest event and provide 
            // custom logging implementation for it
            //context.LogRequest += new EventHandler(OnLogRequest);

            //******** hook up my events *********
            //context.BeginRequest += new EventHandler(this.begin);
            //context.EndRequest += new EventHandler(this.end);
            
        }

        #endregion
        

        private void begin(Object source, EventArgs e)
        {
            IWatchService iWatch = ServiceFactory.WatchService;
            iWatch.Start();
            ((HttpApplication)source).Context.Items.Add(Constants.WatchItemName, iWatch);
        }
        private void end(Object source, EventArgs e)
        {
            var app = (HttpApplication)source;
            IWatchService iWatch = (IWatchService)app.Context.Items[Constants.WatchItemName];
            if (iWatch != null)
            {
                
                var requestUrl = app.Context.Request.Path;
                

                iWatch.Stop(requestUrl);
            }
        }

        public void OnLogRequest(Object source, EventArgs e)
        {
            //custom logging logic can go here
        }
    }
}
