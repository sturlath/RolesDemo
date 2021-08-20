namespace RolesDemo.Permissions
{
    public static class RolesDemoConstants
    {
        // When a user registers that has no tentnat he gets this role that can access certain none-tenant information that the tenants create
        public const string OrdinaryClientRole = "OrdinaryClientRole";

        // When a tenant registers (self service through link (not shown in this demo) he gets this role that just has the things we like the selfservice tenant to have
        public const string SelfServiceTenantClientRole = "SelfServiceTenantClientRole";

    }
}