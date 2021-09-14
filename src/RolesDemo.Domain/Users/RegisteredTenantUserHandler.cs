using Microsoft.AspNetCore.Identity;
using RolesDemo.Permissions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement;
using Volo.Abp.TenantManagement;
using Volo.Abp.Uow;
using Volo.Abp.Users;
using IdentityRole = Volo.Abp.Identity.IdentityRole;

namespace RolesDemo.Users
{
    public class RegisteredTenantUserHandler : IDistributedEventHandler<EntityCreatedEto<UserEto>>, ITransientDependency
    {
        public RegisteredTenantUserHandler(IdentityUserManager identityUserManager,
                            IPermissionManager permissionManager,
                            ICurrentTenant currentTenant,
                            ITenantRepository tenantRepository,
                            IGuidGenerator guidGenerator,
                            IIdentityRoleRepository identityRoleRepository,
                            ILookupNormalizer lookupNormalizer
                           )
        {
            this.identityUserManager = identityUserManager;
            this.permissionManager = permissionManager;
            this.currentTenant = currentTenant;
            this.lookupNormalizer = lookupNormalizer;
            this.tenantRepository = tenantRepository;
            this.guidGenerator = guidGenerator;
            this.identityRoleRepository = identityRoleRepository;
        }

        public IPermissionManager permissionManager { get; }
        private readonly IdentityUserManager identityUserManager;
        private readonly ICurrentTenant currentTenant;
        private readonly ILookupNormalizer lookupNormalizer;
        private readonly ITenantRepository tenantRepository;
        private readonly IGuidGenerator guidGenerator;
        private readonly IIdentityRoleRepository identityRoleRepository;

        [UnitOfWork]
        public async Task HandleEventAsync(EntityCreatedEto<UserEto> eventData)
        {
            await GivePermissionToTenant(eventData); 
            //await GivePermissionToTenant_change_to_null(eventData); 
        }

        private async Task GivePermissionToTenant(EntityCreatedEto<UserEto> eventData)
        {
            using (currentTenant.Change(eventData.Entity.TenantId))
            {
                var roleNotFoundLetsTryAndCreateIt = await identityRoleRepository.FindByNormalizedNameAsync(lookupNormalizer.NormalizeName(RolesDemoConstants.SelfServiceTenantClientRole));
                if (roleNotFoundLetsTryAndCreateIt is null)
                {
                    var newRole = new IdentityRole(guidGenerator.Create(), RolesDemoConstants.SelfServiceTenantClientRole) { IsPublic = true, IsStatic = true };
                    await identityRoleRepository.InsertAsync(newRole, true); //<-- save right away with true so we can get it!
                }

                var stillCantFindTheRoleThisIsNull = await identityRoleRepository.FindByNormalizedNameAsync(lookupNormalizer.NormalizeName(RolesDemoConstants.SelfServiceTenantClientRole));

                // this will fail because no role is found
                await AddRoleToUser(eventData.Entity.Id /*or eventData.Entity.TenantId*/, stillCantFindTheRoleThisIsNull);
            }
        }

        private async Task GivePermissionToTenant_change_to_null(EntityCreatedEto<UserEto> eventData)
        {
            using (currentTenant.Change(null))
            {
                var hereWeFindTheRole = await identityRoleRepository.FindByNormalizedNameAsync(lookupNormalizer.NormalizeName(RolesDemoConstants.SelfServiceTenantClientRole));

                // and this will fail because no entity is found
                await AddRoleToUser(eventData.Entity.Id /*or eventData.Entity.TenantId*/, hereWeFindTheRole);
            }
        }

        private async Task AddRoleToUser(Guid? entityId, IdentityRole stillCantFindTheRoleThisIsNull)
        {
            var permissions = new List<string>
                {
                    RoleDemoPermissions.ClientUserPages,
                };

            foreach (var perm in permissions)
            {
                //this fails because role not found
                await permissionManager.SetForRoleAsync(RolesDemoConstants.SelfServiceTenantClientRole, perm, true);
            }

            //and if role found this fails, with "There is no such an entity"
            var userJustCreated = await identityUserManager.GetByIdAsync(entityId.Value);
            var result = await identityUserManager.AddToRoleAsync(userJustCreated, RolesDemoConstants.SelfServiceTenantClientRole);
        }
    }
}
