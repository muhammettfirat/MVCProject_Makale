
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCProject_Makale.Filter
{
    public class Auth : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
           if(filterContext.HttpContext.Session["login"]==null)
            {
                filterContext.Result = new RedirectResult("/Home/Login");
            }
        }
    }
}