using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace SegundaPracticaMvcCore.Filters
{
    public class AuthorizeUsersAttribute : AuthorizeAttribute
        , IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var usuario = context.HttpContext.User;

            string controller =
                context.RouteData.Values["controller"].ToString();
            string action =
                context.RouteData.Values["action"].ToString();

            ITempDataProvider provider =
                context.HttpContext.RequestServices
                .GetService<ITempDataProvider>();
            
            var TempData = provider.LoadTempData(context.HttpContext);
            
            TempData["controller"] = controller;
            TempData["action"] = action;

            provider.SaveTempData(context.HttpContext, TempData);

            if (usuario.Identity.IsAuthenticated == false)
            {
                RouteValueDictionary loginPath =
                    new RouteValueDictionary
                    (
                        new { controller = "Auth",  action = "Login" }
                    );

                context.Result = 
                    new RedirectToRouteResult( loginPath );
            }
        }
    }
}
