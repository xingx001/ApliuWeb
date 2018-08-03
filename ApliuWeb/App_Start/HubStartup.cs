using System;
using System.Threading.Tasks;
using ApliuTools;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(ApliuWeb.HubStartup))]

namespace ApliuWeb
{
    /// <summary>
    /// 注入自定义Hub 管道模型
    /// </summary>
    public class ErrorHandlingPipelineModule : HubPipelineModule
    {
        /// <summary>
        /// 接收并记录来自Hubs的异常
        /// </summary>
        /// <param name="exceptionContext"></param>
        /// <param name="invokerContext"></param>
        protected override void OnIncomingError(ExceptionContext exceptionContext, IHubIncomingInvokerContext invokerContext)
        {
            Logger.WriteLog("signalR Hubs Exception " + exceptionContext.Error.Message);
            if (exceptionContext.Error.InnerException != null)
            {
                Logger.WriteLog("signalR Hubs Inner Exception " + exceptionContext.Error.InnerException.Message);
            }
            base.OnIncomingError(exceptionContext, invokerContext);
        }
    }

    public class HubStartup
    {
        public void Configuration(IAppBuilder app)
        {
            try
            {
                // Any connection or hub wire up and configuration should go here
                GlobalHost.HubPipeline.AddModule(new ErrorHandlingPipelineModule());

                //开启详细错误反馈
                HubConfiguration hubConfiguration = new HubConfiguration();
                hubConfiguration.EnableDetailedErrors = true;

                // 有关如何配置应用程序的详细信息，请访问 https://go.microsoft.com/fwlink/?LinkID=316888
                app.MapSignalR("/signalr", new HubConfiguration());

                Logger.WriteLog("signalR Hubs 配置完成");
            }
            catch (Exception ex)
            {
                Logger.WriteLog("signalR Hubs 配置失败，详情：" + ex.Message);
            }
        }
    }
}
