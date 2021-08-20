using Volo.Abp.Modularity;

namespace RolesDemo
{
    [DependsOn(
        typeof(RolesDemoApplicationModule),
        typeof(RolesDemoDomainTestModule)
        )]
    public class RolesDemoApplicationTestModule : AbpModule
    {

    }
}