using RolesDemo.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace RolesDemo
{
    [DependsOn(
        typeof(RolesDemoEntityFrameworkCoreTestModule)
        )]
    public class RolesDemoDomainTestModule : AbpModule
    {

    }
}