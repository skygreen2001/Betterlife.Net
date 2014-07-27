using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Portal.Code.Util;

namespace Portal
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // 路由名称
                "{controller}/{action}/{id}", // 带有参数的 URL
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // 参数默认值
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

        /// <summary>
        /// i18n Custom caching
        /// </summary>
        /// <see cref="http://afana.me/post/aspnet-mvc-internationalization-part-2.aspx" title="Output Caching" />
        /// <param name="context"></param>
        /// <param name="arg"></param>
        /// <returns></returns>
        public override string GetVaryByCustomString(HttpContext context, string arg)
        {
            // It seems this executes multiple times and early, so we need to extract language again from cookie.
            if (arg == "culture") // culture name (e.g. "en-US") is what should vary caching
            {
                string cultureName = null;
                // Attempt to read the culture cookie from Request
                HttpCookie cultureCookie = Request.Cookies["_culture"];
                if (cultureCookie != null)
                    cultureName = cultureCookie.Value;
                else
                    cultureName = Request.UserLanguages[0]; // obtain it from HTTP header AcceptLanguages

                // Validate culture name
                cultureName = UtilCultureHelper.GetImplementedCulture(cultureName); // This is safe

                return cultureName.ToLower();// use culture name as cache key, "es", "en-us", "es-cl", etc.
            }

            return base.GetVaryByCustomString(context, arg);
        }
    }
}