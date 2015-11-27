using System.Web;
using System.Web.Mvc;

namespace Zpi2WebApiWithLib
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
