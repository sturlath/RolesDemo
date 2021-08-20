using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace RolesDemo.Data
{
    /* This is used if database provider does't define
     * IRolesDemoDbSchemaMigrator implementation.
     */
    public class NullRolesDemoDbSchemaMigrator : IRolesDemoDbSchemaMigrator, ITransientDependency
    {
        public Task MigrateAsync()
        {
            return Task.CompletedTask;
        }
    }
}