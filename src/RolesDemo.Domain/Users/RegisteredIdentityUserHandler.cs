
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using RolesDemo.Permissions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Uow;
using IdentityRole = Volo.Abp.Identity.IdentityRole;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace RolesDemo.Users
{
    public class RegisteredIdentityUserHandler : IDistributedEventHandler<EntityCreatedEventData<IdentityUser>>, ITransientDependency
    {
        private readonly ILogger logger;

        public RegisteredIdentityUserHandler(IdentityUserManager identityUserManager,
                            IPermissionManager permissionManager,
                            IGuidGenerator guidGenerator,
                            IIdentityRoleRepository identityRoleRepository,
                            ILookupNormalizer lookupNormalizer,
                            ILogger<RegisteredIdentityUserHandler> logger
                           )
        {
            this.identityUserManager = identityUserManager;
            this.permissionManager = permissionManager;
            this.lookupNormalizer = lookupNormalizer;
            this.guidGenerator = guidGenerator;
            this.identityRoleRepository = identityRoleRepository;
            this.logger = logger;
        }

        public IPermissionManager permissionManager { get; }
        private readonly IdentityUserManager identityUserManager;
        private readonly ILookupNormalizer lookupNormalizer;
        private readonly IGuidGenerator guidGenerator;
        private readonly IIdentityRoleRepository identityRoleRepository;

        [UnitOfWork]
        public virtual async Task HandleEventAsync(EntityCreatedEventData<IdentityUser> eventData)
        {
            //If you set a brakepoint here, its not hit when registering user from the main page
            //BUT it gets hit when registering a user when logged in 
            logger.LogInformation("This message is shown in the IdentiyServer console when registering user from the main page, BUT in the HOST console if you are logged in as tenant/host");

            await GivePermissionToUser(eventData.Entity);
        }

        private async Task GivePermissionToUser(IdentityUser user)
        {
            //This is just to show that I can add a USER (not Tenant) to role
            var userJustCreated = await identityUserManager.GetByIdAsync(user.Id);
            var result = await identityUserManager.AddToRoleAsync(userJustCreated, RolesDemoConstants.OrdinaryClientRole);
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
