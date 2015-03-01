using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using web_api_2_filter_poc.Logic;

namespace web_api_2_filter_poc.Filters
{
    public class LogError: ExceptionFilterAttribute
    {        
        private ILogger logger;

        public LogError(Type loggerType)
            : base()
        {
            if (!loggerType.GetInterfaces().Contains(typeof(ILogger)))
                throw new ApplicationException(
                    string.Format("Type {0} does not implement ILogger.",
                        loggerType.Name));

            logger = (ILogger)Activator.CreateInstance(loggerType);
        }

        public override void OnException(HttpActionExecutedContext context)
        {
            logger.Log(context.ActionContext,
                string.Format("{0}: {1}\r\n{2}",
                    context.Exception.GetType().Name,
                    context.Exception.Message,
                    context.Exception.StackTrace));

            base.OnException(context);
        }
    }
}