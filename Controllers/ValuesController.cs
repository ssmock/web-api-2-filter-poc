using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using web_api_2_filter_poc.Filters;
using web_api_2_filter_poc.Logic;

namespace web_api_2_filter_poc.Controllers
{
    [RoutePrefix("api/values")]
    [LogAction(typeof(ConsoleLogger))]
    [LogError(typeof(ConsoleLogger))]
    [Timed(typeof(string))]
    public class ValuesController : ApiController
    {
        [HttpPost]
        [Route("timed")]
        public void PostTimed([FromBody]SleepSpec value)
        {
            Thread.Sleep(value.SleepTimeMs);
        }

        public class SleepSpec
        {
            public int SleepTimeMs { get; set; }
        }

        [HttpPost]
        [Route("always-throws")]
        public void AlwaysThrows()
        {
            throw new ApplicationException(
                string.Format("Error requested at {0}", 
                    DateTime.Now.ToString()));
        }
    }
}
