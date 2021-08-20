using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RolesDemo.Localization;
using RolesDemo.Permissions;
using Volo.Abp.Account.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.UI.Navigation;
using Volo.Abp.Users;

namespace RolesDemo.Blazor.Menus
{
    public class RolesDemoMenuContributor : IMenuContributor
    {
        private readonly IConfiguration _configuration;

        public RolesDemoMenuContributor(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task ConfigureMenuAsync(MenuConfigurationContext context)
        {
            if (context.Menu.Name == StandardMenus.Main)
            {
                await ConfigureMainMenuAsync(context);
            }
            else if (context.Menu.Name == StandardMenus.User)
            {
                await ConfigureUserMenuAsync(context);
            }
        }

        private async Task ConfigureMainMenuAsync(MenuConfigurationContext context)
        {
            var l = context.GetLocalizer<RolesDemoResource>();

            context.Menu.Items.Insert(
                0,
                new ApplicationMenuItem(
                    RolesDemoMenus.Home,
                    l["Menu:Home"],
                    "/",
                    icon: "fas fa-home"
                )
            );

            // Add this for user that registered without providing a tenant
            if (await context.IsGrantedAsync(RoleDemoPermissions.ClientUserPages))
            {
                context.Menu.AddItem(
                    new ApplicationMenuItem(
                        RolesDemoMenus.ClientUserPages,
                        "Ordinary client",
                        url: "/ordinary-client",
                        icon: "fas fa-user-secret")
                );
            }
        }

        private Task ConfigureUserMenuAsync(MenuConfigurationContext context)
        {
            var accountStringLocalizer = context.GetLocalizer<AccountResource>();

            var identityServerUrl = _configuration["AuthServer:Authority"] ?? "";

            context.Menu.AddItem(new ApplicationMenuItem(
                "Account.Manage",
                accountStringLocalizer["MyAccount"],
                $"{identityServerUrl.EnsureEndsWith('/')}Account/Manage?returnUrl={_configuration["App:SelfUrl"]}",
                icon: "fa fa-cog",
                order: 1000,
                null).RequireAuthenticated());

            return Task.CompletedTask;
        }
    }
}
