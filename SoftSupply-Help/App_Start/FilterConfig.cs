﻿using System.Web;
using System.Web.Mvc;

namespace SoftSupply.Help
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
