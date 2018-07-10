using ApliuTools;
using ApliuWeb.WeChart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.SessionState;

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
                Logger.WriteLogWeb("主程序开始启动");
                AreaRegistration.RegisterAllAreas();

                WebApiConfig.Register(GlobalConfiguration.Configuration);
                FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
                RouteConfig.RegisterRoutes(RouteTable.Routes);
                BundleConfig.RegisterBundles(BundleTable.Bundles);

                //加载配置文件
                SiteConfig.LoadConfig();

                //初始化程序跟目录
                Common.RootDirectory = HttpContext.Current.Server.MapPath("~");

                //启动access_token管理任务
                WxTokenManager.TokenTaskStart();

                //创建自定义菜单
                WxDefaultMenu.CreateMenus();

                Logger.WriteLogWeb("主程序启动完成");
            }
            catch (Exception ex)
            {
                Logger.WriteLogWeb("主程序启动失败，详情：" + ex.Message);
            }
        }

        protected void Application_PostAuthorizeRequest()
        {
            HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
        }
    }
}