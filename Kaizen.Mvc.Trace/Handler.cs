using Kaizen.Performance.Service;
using Kaizen.Performance.Service.Interface;
using Performance.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;

namespace Kaizen.Mvc.Trace
{
    public class Handler : IHttpHandler
    {
        /// <summary>
        /// You will need to configure this handler in the Web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpHandler Members
        public bool IsReusable
        {
            // Return false in case your Managed Handler cannot be reused for another request.
            // Usually this would be false in case you have some state information preserved per request.
            get { return true; }
        }
        IWatchService iWatch;
        public void ProcessRequest(HttpContext context)
        {
            //write your handler implementation here.
            var req = context.Request;
            var response = context.Response;
            using(HtmlTextWriter writer = new HtmlTextWriter(response.Output))
            {
                writer.Write("<!DOCTYPE html>");
                writer.Write("<html>");
                writer.Write("<head>");
                writer.Write("<meta charset=\"utf - 8\" /><meta name=\"viewport\" content=\"width = device - width, initial - scale = 1.0\">");
                writer.Write("<title>Kaizen Trace Report</title>");
                writer.Write("</head>");
                writer.Write("<body>");
                if (ApplicationStartCode.DisableKaizenMvcTrace)
                {
                    writer.Write("<h2>Kaizen Mvc Trace is disabled. Please contact administrator..</h2>");
                }
                else
                {
                    iWatch = ServiceFactory.WatchService;
                    ReportType reportType;
                    if (!Enum.TryParse<ReportType>(req.QueryString["reportType"], out reportType) || !Enum.IsDefined(typeof(ReportType),reportType))
                        reportType = ReportType.Milliseconds; // default reportType
                    this.GenerateHtmlTabularReport(writer, reportType);
                }
                writer.Write("</body>");
                writer.Write("</html>");
            }
            response.End();
            //if(req.HttpMethod.ToUpper() == "POST")
            //{
            //    var status = req["status"];
            //    if (status != null)
            //    {
            //        ActionFilter.IsDisabled = status == "enabled" ? false : true;
            //    }
            //    response.Redirect(req.Path);
            //}
        }

        
        private void GenerateHtmlTabularReport(TextWriter writer, ReportType reportType)
        {
            var collection = Enumerable.OrderByDescending(iWatch.GetConsolidateLogs(reportType), c=> c.Avg);
            ////https://www.dotnetperls.com/htmltextwriter
            writer.Write("<table border=\"1\">");

            writer.Write("<thead>");
            writer.Write("<tr>");
            writer.Write("<th>Url</th>");
            writer.Write("<th>Count</th>");
            writer.Write($"<th>Avg-{reportType.ToString()}</th>");
            writer.Write($"<th>Max-{reportType.ToString()}</th>");
            writer.Write($"<th>Min-{reportType.ToString()}</th>");
            writer.Write("</tr>");
            writer.Write("</thead>");

            writer.Write("<tbody>");
            foreach (var item in collection)
            {
                writer.Write("<tr>");
                writer.Write($"<td>{item.Url}</td>");
                writer.Write($"<td>{item.Count}</td>");
                writer.Write($"<td>{item.Avg}</td>");
                writer.Write($"<td>{item.Max}</td>");
                writer.Write($"<td>{item.Min}</td>");
                writer.Write("</tr>");
            }// close for loop
            writer.Write("</tbody>");
            writer.Write("</table>");
        }

        private void generateForm(HttpResponse res)
        {
            res.Write("<form method=\"post\">");
            res.Write("<table>");
           
            res.Write("<tr>");
            res.Write("<td>Status:</td>");
            res.Write("<td>");
            
            string radio1 = $"<input type=\"radio\" name=\"status\" value=\"enabled\" {(ActionFilter.IsDisabled ? string.Empty : "checked")} />Enabled &nbsp;";
            string radio2 = $"<input type=\"radio\" name=\"status\" value=\"disabled\" {(ActionFilter.IsDisabled ? "checked" : string.Empty)} />Disabled &nbsp;";
            res.Write(radio1);
            res.Write(radio2);
            res.Write("<td>&nbsp;</td>");
            res.Write("<td>&nbsp;</td>");
            res.Write("<td><input type=\"submit\" value=\"Save\" /></td>");
            res.Write("<td><button type=\"button\" onclick=\"location.reload();\">Cancel</button></td>");
            res.Write("</tr>");

            //res.Write("<tr>");
            //res.Write("<td><input type=\"submit\" value=\"Save\" /></td>");
            //res.Write("<td><button type=\"button\" onclick=\"location.reload();\">Cancel</button></td>");
            //res.Write("</tr>");

            res.Write("</table>");
            res.Write("</form>");
        }

        #endregion
    }
}
