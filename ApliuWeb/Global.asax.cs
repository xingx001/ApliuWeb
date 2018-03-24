using ApliuTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ApliuWeb
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            try
            {
                AreaRegistration.RegisterAllAreas();

                WebApiConfig.Register(GlobalConfiguration.Configuration);
                FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
                RouteConfig.RegisterRoutes(RouteTable.Routes);
                BundleConfig.RegisterBundles(BundleTable.Bundles);

                SiteConfig.LoadConfig();

                DataAccess.Instance.Ceshi();
                //SMSMessage sms = new TencentSMS();
                //string msg;
                //bool result = sms.SendSMS("18779182730", "验证码：100000，有效期1分钟", out msg, SiteConfig.GetConfigNodeValue("SMSAppId"), SiteConfig.GetConfigNodeValue("SMSAppKey"));

                Logger.WriteLogWeb("主程序启动完成201893241942");
            }
            catch (Exception ex)
            {
                Logger.WriteLogWeb("主程序启动失败，详情：" + ex.Message);
            }
        }
    }
}