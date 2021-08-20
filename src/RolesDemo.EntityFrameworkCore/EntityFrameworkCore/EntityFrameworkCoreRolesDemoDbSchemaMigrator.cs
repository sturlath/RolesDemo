using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RolesDemo.Data;
using Volo.Abp.DependencyInjection;

namespace RolesDemo.EntityFrameworkCore
{
    public class EntityFrameworkCoreRolesDemoDbSchemaMigrator
        : IRolesDemoDbSchemaMigrator, ITransientDependency
    {
        private readonly IServiceProvider _serviceProvider;

        public EntityFrameworkCoreRolesDemoDbSchemaMigrator(
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task MigrateAsync()
        {
            /* We intentionally resolving the RolesDemoDbContext
             * from IServiceProvider (instead of directly injecting it)
             * to properly get the connection string of the current tenant in the
             * current scope.
             */

            await _serviceProvider
                .GetRequiredService<RolesDemoDbContext>()
                .Database
                .MigrateAsync();
        }
    }
}
