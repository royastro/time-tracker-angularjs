using System.Web;
using System.Web.Mvc;
using TimeTracker.Services.Filters;

namespace TimeTracker.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}