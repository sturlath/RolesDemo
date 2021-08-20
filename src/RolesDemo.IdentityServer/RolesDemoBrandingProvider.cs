using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace RolesDemo
{
    [Dependency(ReplaceServices = true)]
    public class RolesDemoBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "RolesDemo";
    }
}
