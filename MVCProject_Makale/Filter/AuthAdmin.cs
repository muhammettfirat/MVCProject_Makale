using Makale.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCProject_Makale.Filter
{
    public class AuthAdmin : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            Kullanici user = (Kullanici)filterContext.HttpContext.Session["login"];
            if (user != null && user.IsAdmin==false)
            {
               filterContext.Result = new RedirectResult("/Home/HataliErisim");
            }
        }
    }
}