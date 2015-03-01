using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using web_api_2_filter_poc.Logic;

namespace web_api_2_filter_poc.Filters
{
    public class LogAction : ActionFilterAttribute
    {
        private ILogger logger;

        public LogAction(Type loggerType)
            : base()
        {
            if (!loggerType.GetInterfaces().Contains(typeof(ILogger)))
                throw new ApplicationException(
                    string.Format("Type {0} does not implement ILogger.",
                        loggerType.Name));

            logger = (ILogger)Activator.CreateInstance(loggerType);
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            logger.Log(actionContext, 
                string.Format("Called at {0}", DateTime.Now.ToString()));

            base.OnActionExecuting(actionContext);
        }
    }
}