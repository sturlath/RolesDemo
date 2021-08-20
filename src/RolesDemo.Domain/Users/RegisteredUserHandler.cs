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
    public class RegisteredUserHandler : IDistributedEventHandler<EntityCreatedEto<UserEto>>, ITransientDependency
    {
        public RegisteredUserHandler(IdentityUserManager identityUserManager,
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


        //I don't know why but I'm not hitting this in debug mode
        [UnitOfWork]
        public async Task HandleEventAsync(EntityCreatedEto<UserEto> eventData)
        {
            var isTenant = eventData.Entity.TenantId.HasValue;

            if (isTenant)
            {              
                await GivePermissionToTenant(eventData); //<-- fails
                //await GivePermissionToTenant_change_to_null(eventData); //<-- fails
            }
            else
            {
                await GivePermissionToUser(eventData);
            }
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

        private async Task GivePermissionToUser(EntityCreatedEto<UserEto> eventData)
        {
            //This is just to show that I can add a USER (not Tenant) to role
            var userJustCreated = await identityUserManager.GetByIdAsync(eventData.Entity.Id);
            var result = await identityUserManager.AddToRoleAsync(userJustCreated, RolesDemoConstants.OrdinaryClientRole);

            //For some reason this throws this error (when running migration) but just in this demo app! I can´t see my other project is doing anything differently!
            //"Caught an exception while publishing the event 'Volo.Abp.Domain.Entities.Events.EntityCreatedEventData"
            //"A DbContext can only be created inside a unit of work! Volo.Abp.AbpException: A DbContext can only be created inside a unit of work!"
        }
    }
}
