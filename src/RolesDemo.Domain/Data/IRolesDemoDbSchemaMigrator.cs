using System.Threading.Tasks;

namespace RolesDemo.Data
{
    public interface IRolesDemoDbSchemaMigrator
    {
        Task MigrateAsync();
    }
}
