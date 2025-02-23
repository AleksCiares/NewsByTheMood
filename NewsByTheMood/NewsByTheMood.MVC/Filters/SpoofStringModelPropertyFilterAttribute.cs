using EFCoreSampleApp;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NewsByTheMood.MVC.Filters
{
    public class SpoofStringModelPropertyFilterAttribute : Attribute, IActionFilter
    {
        private readonly bool _isSpoof;
        private readonly string _modelPropertyName;
        private readonly AlphabetCrypt _alphabetCrypt;
        private readonly Type _modelType;

        public SpoofStringModelPropertyFilterAttribute(bool isSpoof, Type modelType, string modelPropertyName, string secret)
        {
            this._isSpoof = isSpoof;
            this._modelPropertyName = modelPropertyName;
            this._alphabetCrypt = new AlphabetCrypt(secret);
            this._modelType = modelType;
        }

        public void OnActionExecuting(ActionExecutingContext context) {}
        public void OnActionExecuted(ActionExecutedContext context) 
        {
            var contextResult = context.Result?.GetType();
            if (contextResult is not null)
            {
                var contextModel = contextResult.GetProperties().Select(prop => prop.Name.Equals("Model"));
                if (contextModel is not null)
                {
                    if (contextModel.GetType() == this._modelType.GetType()) 
                    {
                        if (this._modelType.GetType().IsArray)
                        {
                            foreach (var item in contextModel)
                            {
                                
                            }
                        }
                    }
                }
            }
        }
    }
}
