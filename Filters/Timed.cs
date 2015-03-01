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
    /// <summary>
    /// Provides synchronous logging of the elapsed time from OnActionExecting
    /// to OnActionExecuted.
    /// </summary>
    /// <remarks>
    /// One nice thing about our simple logger is: it captures the controller
    /// and action names.
    /// 
    /// If our logging operation requires asynchronous processing, swap 
    /// OnActionExecuted for OnActionExecutedAsync.  (Do not include both 
    /// overrides!  This will result in both of them being called, and elapsed
    /// times being double-logged.)
    /// </remarks>
    public class Timed : ActionFilterAttribute
    {
        private const string TIMED_PROPERTY_KEY = "Filters::Timed";
        private const string TIMED_KEY_FORMAT = "Filters::Timed::ID={0}";
        private const string TIMED_LOG_DESCRIPTION_FORMAT = "Elapsed: {0}";

        private Dictionary<string, Stopwatch> stopwatches;
        private object dictionaryLocker = new object();
        private ILogger logger;

        public Timed(Type loggerType) : base()
        {
            if (!loggerType.GetInterfaces().Contains(typeof(ILogger)))
                throw new TypeLoadException(
                    string.Format("Type {0} does not implement ILogger.",
                        loggerType.Name));

            stopwatches = new Dictionary<string, Stopwatch>();
            logger = (ILogger)Activator.CreateInstance(loggerType);
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            StartTiming(actionContext);

            base.OnActionExecuting(actionContext);
        }

        public override void OnActionExecuted(
            HttpActionExecutedContext actionExecutedContext)
        {
            LogTime(actionExecutedContext.ActionContext);

            base.OnActionExecuted(actionExecutedContext);
        }

        ///// <remarks>
        ///// Remember to remove OnActionExecuted if you include this.
        ///// </remarks>
        //public override System.Threading.Tasks.Task OnActionExecutedAsync
        //    (HttpActionExecutedContext actionExecutedContext,
        //    CancellationToken cancellationToken)
        //{
        //    if (!CheckCancellation(cancellationToken,
        //        actionExecutedContext.ActionContext))
        //    {
        //        LogTime(actionExecutedContext.ActionContext);
        //    }

        //    return base.OnActionExecutedAsync(
        //        actionExecutedContext, cancellationToken);
        //}

        private void LogTime(HttpActionContext actionContext)
        {
            var timerKey = GetTimerKeyOrNull(actionContext);
            var timer = stopwatches[timerKey];

            timer.Stop();

            stopwatches.Remove(timerKey);

            logger.Log(actionContext, GetTimerDescription(timer));
        }

        private void StartTiming(HttpActionContext actionContext)
        {
            var timer = new Stopwatch();
            var timerKey = MakeTimerKey();

            // Due to rare threading problems with dictionary insertions.
            lock (dictionaryLocker)
            {
                stopwatches.Add(timerKey, timer);
            }

            timer.Start();

            actionContext.Request.Properties[TIMED_PROPERTY_KEY] = timerKey;
        }
               
        private string MakeTimerKey()
        {
            return string.Format(TIMED_KEY_FORMAT, Guid.NewGuid());
        }

        private void CancelTimer(string key)
        {
            if (key != null)
            {
                stopwatches[key].Stop();
                stopwatches.Remove(key);
            }
        }

        private bool CheckCancellation(
            CancellationToken cancellationToken,
            HttpActionContext actionContext)
        {
            var cancelled = false;

            if (cancellationToken.IsCancellationRequested)
            {
                var key = GetTimerKeyOrNull(actionContext);

                CancelTimer(key);

                cancelled = true;
            }

            return cancelled;
        }

        private string GetTimerKeyOrNull(HttpActionContext actionContext)
        {
            return actionContext.Request
                .Properties[TIMED_PROPERTY_KEY] as String;
        }

        private string GetTimerDescription(Stopwatch timer)
        {
            return string.Format(TIMED_LOG_DESCRIPTION_FORMAT, timer.Elapsed);
        }
    }
}