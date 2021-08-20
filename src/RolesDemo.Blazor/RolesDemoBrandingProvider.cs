using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace RolesDemo.Blazor
{
    [Dependency(ReplaceServices = true)]
    public class RolesDemoBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "RolesDemo";
    }
}
