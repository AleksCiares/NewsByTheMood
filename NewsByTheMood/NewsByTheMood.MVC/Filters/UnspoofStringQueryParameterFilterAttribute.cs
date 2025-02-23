using EFCoreSampleApp;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NewsByTheMood.MVC.Filters
{
    public class UnspoofStringQueryParameterFilterAttribute : Attribute, IActionFilter
    {
        private readonly bool _isSpoof;
        private readonly string _queryParamName;
        private readonly AlphabetCrypt _alphabetCrypt;

        public UnspoofStringQueryParameterFilterAttribute(bool isSpoof, string queryParamName, string secret)
        {
            this._isSpoof = isSpoof;
            this._queryParamName = queryParamName;
            this._alphabetCrypt = new AlphabetCrypt(secret);
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (_isSpoof && context.ActionArguments[this._queryParamName] is not null
                && context.ActionArguments[this._queryParamName] is string param)
            {
                context.ActionArguments[this._queryParamName] = 
                    this._alphabetCrypt.Deobfuscate(param);
            }
            
        }
        public void OnActionExecuted(ActionExecutedContext context){}
    }
}
