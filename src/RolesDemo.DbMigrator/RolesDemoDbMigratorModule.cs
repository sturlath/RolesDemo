using RolesDemo.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Modularity;

namespace RolesDemo.DbMigrator
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(RolesDemoEntityFrameworkCoreModule),
        typeof(RolesDemoApplicationContractsModule)
        )]
    public class RolesDemoDbMigratorModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpBackgroundJobOptions>(options => options.IsJobExecutionEnabled = false);
        }
    }
}
