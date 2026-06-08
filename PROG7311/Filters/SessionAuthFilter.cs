using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PROG7311.Filters
{
    public class SessionAuthFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var controller = context.ActionDescriptor.RouteValues["controller"]?.ToString();
            if (string.Equals(controller, "Auth", System.StringComparison.OrdinalIgnoreCase))
                return;

            var hasToken = context.HttpContext.Session.GetString("JwtToken");
            if (string.IsNullOrEmpty(hasToken))
            {
                context.Result = new RedirectToActionResult("Login", "Auth", null);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
