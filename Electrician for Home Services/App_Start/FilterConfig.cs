﻿using System.Web;
using System.Web.Mvc;

namespace Electrician_for_Home_Services
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
