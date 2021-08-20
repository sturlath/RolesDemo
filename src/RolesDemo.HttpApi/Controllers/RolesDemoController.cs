using RolesDemo.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace RolesDemo.Controllers
{
    /* Inherit your controllers from this class.
     */
    public abstract class RolesDemoController : AbpController
    {
        protected RolesDemoController()
        {
            LocalizationResource = typeof(RolesDemoResource);
        }
    }
}