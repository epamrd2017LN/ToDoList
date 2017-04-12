using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using todoclient.Infrastructure;

namespace todoclient
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private static readonly int defaultSyncTime = 60;

        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            int syncTime;
            if (!Int32.TryParse(ConfigurationManager.AppSettings["SyncTimeInSeconds"], out syncTime))
            {
                syncTime = defaultSyncTime;
            }
            Scheduler.AddTask("sync", syncTime);
        }
    }
}
