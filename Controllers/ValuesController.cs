using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using web_api_2_filter_poc.Filters;

namespace web_api_2_filter_poc.Controllers
{
    [RoutePrefix("api/values")]
    public class ValuesController : ApiController
    {
        [HttpPost]
        [Timed]
        [Route("timed")]
        public void PostTimed([FromBody]SleepSpec value)
        {
            Thread.Sleep(value.SleepTimeMs);
        }

        public class SleepSpec
        {
            public int SleepTimeMs { get; set; }
        }
    }
}
