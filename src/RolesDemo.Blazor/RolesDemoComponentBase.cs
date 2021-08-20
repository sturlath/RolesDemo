using RolesDemo.Localization;
using Volo.Abp.AspNetCore.Components;

namespace RolesDemo.Blazor
{
    public abstract class RolesDemoComponentBase : AbpComponentBase
    {
        protected RolesDemoComponentBase()
        {
            LocalizationResource = typeof(RolesDemoResource);
        }
    }
}
