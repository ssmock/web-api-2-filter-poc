using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;

namespace web_api_2_filter_poc.Logic
{
    public interface ILogger
    {
        void Log(string message);

        void Log(HttpActionContext context, string description);
    }
}
