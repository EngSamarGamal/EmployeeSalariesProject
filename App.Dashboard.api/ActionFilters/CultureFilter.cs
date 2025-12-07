using Microsoft.AspNetCore.Mvc.Filters;
using System.Globalization;

namespace App.Dashboard.api.ActionFilters
{
    public class CultureFilter
         : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var lang = context.HttpContext.Request.Headers["language"].FirstOrDefault() ?? "en";
            var culture = new CultureInfo(lang);

            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
