using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SegundaPracticaMvcCore.Filters
{
    public class AuthorizeUsersAttribute : AuthorizeAttribute
        , IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var usuario = context.HttpContext.User;

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
