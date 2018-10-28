using System.Web.Http;
using System.Web.Mvc;

namespace RRL.WEB.Areas.WebApi
{
    /// <summary>
    /// 默认的WebApi区域注册
    /// </summary>
    public class WebApiAreaRegistration : AreaRegistration
    {
        /// <summary>
        /// 区域名
        /// </summary>
        public override string AreaName
        {
            get
            {
                return "GameApi";
            }
        }

        /// <summary>
        /// 注册主体
        /// </summary>
        /// <param name="context"></param>
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.Routes.MapHttpRoute(
                name: "GameApi_default",
                routeTemplate: "GameApi/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}