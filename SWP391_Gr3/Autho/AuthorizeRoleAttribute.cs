using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SWP391_Gr3.Autho
{
    public class AuthorizeRoleAttribute :Attribute, IPageFilter
    {
        private readonly string[] _roles;

        public AuthorizeRoleAttribute(string roles)
        {
            _roles = roles.Split(',').Select(r => r.Trim()).ToArray();
        }

        public void OnPageHandlerExecuted(PageHandlerExecutedContext context)
        {
          //  throw new NotImplementedException();
        }

        public void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            var userRole = context.HttpContext.Session.GetString("UserRole");

            if (string.IsNullOrEmpty(userRole) || !_roles.Contains(userRole))
            {
                context.Result = new RedirectToPageResult("/Home/AccessDenied");
            }
        }

        public void OnPageHandlerSelected(PageHandlerSelectedContext context)
        {
            
        }
    }
}
