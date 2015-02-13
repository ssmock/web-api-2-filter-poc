using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace web_api_2_filter_poc.Filters
{
    public class LoggingFilter: ActionFilterAttribute
    {
        public LoggingFilter() : base()
        {
            // Nothing
        }

        protected void Log(HttpActionContext context, string description)
        {
            Debug.WriteLine("{0} - {1}.{2} - {3}",
                   this.GetType().Name,
                   context.ActionDescriptor.ControllerDescriptor.ControllerName,
                   context.ActionDescriptor.ActionName,
                   description);
        }
    }
}