using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NewsByTheMood.MVC.Filters
{
    // Filter to check whether a string parameter can be converted to an integer
    public class ValidateStringToIntQueryParameterFilterAttribute : Attribute, IActionFilter
    {
        private readonly string _paramName = "";

        public ValidateStringToIntQueryParameterFilterAttribute(string paramName)
        {
            this._paramName = paramName;
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            Int64 value = 0;
            if (context.ActionArguments.ContainsKey(this._paramName) &&
                context.ActionArguments[this._paramName] is string param &&
                Int64.TryParse(param, out value))
            {
                context.ActionArguments[this._paramName] = value;
            }
            else
            {
                context.Result = new ContentResult
                {
                    StatusCode = 400
                };
            }
        }

        public void OnActionExecuted(ActionExecutedContext context){}
    }
}
