using Microsoft.AspNetCore.Identity;
using RolesDemo.Permissions;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Uow;
using IdentityRole = Volo.Abp.Identity.IdentityRole;

namespace RolesDemo.DbMigrator.DataSeed
{
    public class RoleDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IIdentityRoleRepository identityRoleRepository;
        private readonly IPermissionManager permissionManager;
        private readonly ILookupNormalizer lookupNormalizer;
        private readonly IGuidGenerator guidGenerator;


        public RoleDataSeedContributor(IIdentityRoleRepository identityRoleRepository, IPermissionManager permissionManager, ILookupNormalizer lookupNormalizer, IGuidGenerator guidGenerator)
        {
            this.identityRoleRepository = identityRoleRepository;
            this.permissionManager = permissionManager;
            this.lookupNormalizer = lookupNormalizer;
            this.guidGenerator = guidGenerator;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            await AddOrdinaryClientRoleAsync(context);
            await AddSelfServiceTenantRoleAsync(context);
        }

        private async Task AddOrdinaryClientRoleAsync(DataSeedContext context)
        {
            // Don't add this role when if we are seeding a tenant
            if (context.TenantId is null)
            {
                var role = await identityRoleRepository.FindByNormalizedNameAsync(lookupNormalizer.NormalizeName(RolesDemoConstants.OrdinaryClientRole));
                if (role is not null)
                {
                    return;
                }

                // Create the role and insert it
                var newRole = new IdentityRole(guidGenerator.Create(), RolesDemoConstants.OrdinaryClientRole) { IsPublic = true, IsStatic = true };
                await identityRoleRepository.InsertAsync(newRole);
            }
        }

        private async Task AddSelfServiceTenantRoleAsync(DataSeedContext context)
        {
            var role = await identityRoleRepository.FindByNormalizedNameAsync(lookupNormalizer.NormalizeName(RolesDemoConstants.SelfServiceTenantClientRole));
            if (role is not null)
            {
                return;
            }

            // Create the role and insert it
            var newRole = new IdentityRole(guidGenerator.Create(), RolesDemoConstants.SelfServiceTenantClientRole) { IsPublic = true, IsStatic = true };
            await identityRoleRepository.InsertAsync(newRole, true);

            // Here the roles get created
            var identityRoles = await identityRoleRepository.GetListAsync();
            var selfServiceTenantClientRole_was_created = await identityRoleRepository.FindByNormalizedNameAsync(lookupNormalizer.NormalizeName(RolesDemoConstants.SelfServiceTenantClientRole));
            var ordinaryClientRole_was_created = await identityRoleRepository.FindByNormalizedNameAsync(lookupNormalizer.NormalizeName(RolesDemoConstants.OrdinaryClientRole));
        }
    }
}
