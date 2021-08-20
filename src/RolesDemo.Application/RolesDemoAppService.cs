using System;
using System.Collections.Generic;
using System.Text;
using RolesDemo.Localization;
using Volo.Abp.Application.Services;

namespace RolesDemo
{
    /* Inherit your application services from this class.
     */
    public abstract class RolesDemoAppService : ApplicationService
    {
        protected RolesDemoAppService()
        {
            LocalizationResource = typeof(RolesDemoResource);
        }
    }
}
