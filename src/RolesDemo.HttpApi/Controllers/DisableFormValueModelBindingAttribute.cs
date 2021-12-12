using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace RolesDemo.Controllers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class DisableFormValueModelBindingAttribute : Attribute, IResourceFilter
    {
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            var factories = context.ValueProviderFactories;
            factories.RemoveType<FormValueProviderFactory>();
            factories.RemoveType<FormFileValueProviderFactory>();
            factories.RemoveType<JQueryFormValueProviderFactory>();
            factories.RemoveType<RouteValueProviderFactory>();
            factories.RemoveType<QueryStringValueProviderFactory>();
            
            context.ValueProviderFactories.Clear();
        }
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
        }
    }
}
