using System.Web;
using System.Web.Mvc;

namespace Polizia_Municipale_CRM
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
