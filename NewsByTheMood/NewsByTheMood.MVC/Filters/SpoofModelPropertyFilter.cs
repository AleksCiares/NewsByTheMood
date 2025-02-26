using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using NewsByTheMood.Core.Crypto;
using NewsByTheMood.MVC.Options;

namespace NewsByTheMood.MVC.Filters
{
    

    // Filter for spoofing certian HttpContext Model
    public class SpoofModelPropertyFilter : IActionFilter
    {
        private readonly bool _isSpoof = false;
        private readonly string[]? _pathToProperty = null;
        private readonly AlphabetCrypt? _alphabetCrypt = null;

        public SpoofModelPropertyFilter(IOptions<SpoofOptions> configuration, string pathToProperty) 
        {
            if (this._isSpoof = configuration.Value.SpoofRealId)
            {
                if (this._isSpoof &&
                    !string.IsNullOrEmpty(pathToProperty))
                {
                    this._pathToProperty = pathToProperty.Split('.');
                    this._alphabetCrypt = new AlphabetCrypt(configuration.Value.SpoofSecret);
                }
                else
                {
                    this._isSpoof = false;
                    this._pathToProperty = null;
                    this._alphabetCrypt = null;
                }               
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (this._isSpoof)
            {
                var paramName = this._pathToProperty![this._pathToProperty.Length - 1];
                if (context.ActionArguments.ContainsKey(paramName) &&
                    context.ActionArguments[paramName] is string param)
                {
                    context.ActionArguments[paramName] =
                        this._alphabetCrypt!.Encrypt(param);
                }
            }
            
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (this._isSpoof)
            {
                var model = (context.Result as ViewResult)?.Model;
                if (model != null)
                {
                    var type = model.GetType();
                    for (var i = 0; i < this._pathToProperty!.Length-1; i++)
                    {
                        model = type.GetProperty(this._pathToProperty[i], 
                            BindingFlags.IgnoreCase | 
                            BindingFlags.Instance | 
                            BindingFlags.Public)?.
                            GetValue(model);
                        if (model == null)
                        {
                            return;
                        }
                        type = model.GetType();
                    }

                    if (type.IsArray)
                    {
                        PropertyInfo? property = null;
                        object? value = null;
                        foreach (var item in (Array)model)
                        {
                            property = item.GetType().GetProperty(this._pathToProperty[this._pathToProperty.Length - 1], 
                                BindingFlags.IgnoreCase | 
                                BindingFlags.Instance | 
                                BindingFlags.Public);
                            if (property == null)
                            {
                                continue;
                            }
                            value = property.GetValue(item) as string;
                            if (value == null)
                            {
                                continue;
                            }
                            property.SetValue(item, this._alphabetCrypt!.Crypt((string)value));
                        }
                    }
                    else
                    {
                        var property = type.GetProperty(this._pathToProperty[this._pathToProperty.Length - 1], 
                            BindingFlags.IgnoreCase | 
                            BindingFlags.Instance |
                            BindingFlags.Public);
                        if (property == null)
                        {
                            return;
                        }
                        var value = property.GetValue(model) as string;
                        if (value == null)
                        {
                            return;
                        }
                        property.SetValue(model, this._alphabetCrypt!.Crypt((string)value));
                    }
                }
            }
        }

        
    }

}
