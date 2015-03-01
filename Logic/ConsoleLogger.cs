using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Diagnostics;

namespace web_api_2_filter_poc.Logic
{
    public class ConsoleLogger : ILogger
    {
        public void Log(string message)
        {
            Debug.WriteLine(message);
        }


        public void Log(HttpActionContext context, string description)
        {
            Debug.WriteLine("{0} - {1}.{2} - {3}",
                   this.GetType().Name,
                   context.ActionDescriptor.ControllerDescriptor.ControllerName,
                   context.ActionDescriptor.ActionName,
                   description);
        }
    }
}