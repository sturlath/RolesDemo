using RolesDemo.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace RolesDemo.Permissions
{
    public class RolesDemoPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup(RoleDemoPermissions.GroupName);

            myGroup.AddPermission(RoleDemoPermissions.Dashboard.Host, L("Permission:Dashboard"), MultiTenancySides.Host);
            myGroup.AddPermission(RoleDemoPermissions.Dashboard.Tenant, L("Permission:Dashboard"), MultiTenancySides.Tenant);

            //Only registered users with NO specified tenant, get this permission and should see a "Ordinary client" menu item!
            myGroup.AddPermission(RoleDemoPermissions.ClientUserPages, L("Permission:ClientPages"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<RolesDemoResource>(name);
        }
    }
}
