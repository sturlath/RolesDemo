namespace RolesDemo.Permissions
{
    public static class RoleDemoPermissions
    {
        public const string GroupName = "RolesDemo";

        public static class Dashboard
        {
            public const string DashboardGroup = GroupName + ".Dashboard";
            public const string Host = DashboardGroup + ".Host";
            public const string Tenant = DashboardGroup + ".Tenant";
        }


        public const string ClientUserPages = GroupName + ".ClientUserPages";
    }
}
