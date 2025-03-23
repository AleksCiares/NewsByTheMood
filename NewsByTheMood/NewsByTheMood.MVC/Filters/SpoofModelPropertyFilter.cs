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
        private readonly ILogger<SpoofModelPropertyFilter> _logger;

        public SpoofModelPropertyFilter(IOptions<SpoofOptions> options, string pathToProperty, ILogger<SpoofModelPropertyFilter> logger)
        {
            _logger = logger;

            try
            {
                if (_isSpoof = options.Value.SpoofRealId)
                {
                    if (_isSpoof && !string.IsNullOrEmpty(pathToProperty))
                    {
                        _pathToProperty = pathToProperty.Split('.');
                        _alphabetCrypt = new AlphabetCrypt(options.Value.SpoofSecret);
                    }
                    else
                    {
                        _isSpoof = false;
                        _pathToProperty = null;
                        _alphabetCrypt = null;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while initializing SpoofModelPropertyFilter");
                _isSpoof = false;
                _pathToProperty = null;
                _alphabetCrypt = null;
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            string? paramName = null;
            if (_isSpoof)
            {
                try
                {
                    paramName = _pathToProperty![_pathToProperty.Length - 1];
                    if (context.ActionArguments.ContainsKey(paramName) &&
                        context.ActionArguments[paramName] is string param)
                    {
                        context.ActionArguments[paramName] = _alphabetCrypt!.Encrypt(param);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error while SpoofModelPropertyFilter. Parameter: {paramName}");
                }
            }

        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (_isSpoof)
            {
                try
                {
                    var model = (context.Result as ViewResult)?.Model;
                    if (model != null)
                    {
                        var type = model.GetType();
                        for (var i = 0; i < _pathToProperty!.Length - 1; i++)
                        {
                            model = type.GetProperty(_pathToProperty[i],
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
                                property = item.GetType().GetProperty(_pathToProperty[_pathToProperty.Length - 1],
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
                                property.SetValue(item, _alphabetCrypt!.Crypt((string)value));
                            }
                        }
                        else
                        {
                            var property = type.GetProperty(_pathToProperty[_pathToProperty.Length - 1],
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
                            property.SetValue(model, _alphabetCrypt!.Crypt((string)value));
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while SpoofModelPropertyFilter");
                }
            }
        }
    }
}
