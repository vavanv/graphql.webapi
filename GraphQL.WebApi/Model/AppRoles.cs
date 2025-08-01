namespace GraphQL.WebApi.Model
{
    public static class AppRoles
    {
        public const string Admin = "Admin";
        public const string Manager = "Manager";
        public const string User = "User";
        public const string Guest = "Guest";

        public static readonly string[] AllRoles = { Admin, Manager, User, Guest };
    }

    public static class AppPermissions
    {
        // Customer permissions
        public const string ViewCustomers = "ViewCustomers";
        public const string CreateCustomer = "CreateCustomer";
        public const string EditCustomer = "EditCustomer";
        public const string DeleteCustomer = "DeleteCustomer";

        // User management permissions
        public const string ViewUsers = "ViewUsers";
        public const string CreateUser = "CreateUser";
        public const string EditUser = "EditUser";
        public const string DeleteUser = "DeleteUser";

        // Admin permissions
        public const string ManageRoles = "ManageRoles";
        public const string SystemAdmin = "SystemAdmin";
    }

    public static class RolePermissions
    {
        public static readonly Dictionary<string, string[]> RolePermissionMap = new()
        {
            [AppRoles.Admin] = new[]
            {
                AppPermissions.ViewCustomers, AppPermissions.CreateCustomer, AppPermissions.EditCustomer, AppPermissions.DeleteCustomer,
                AppPermissions.ViewUsers, AppPermissions.CreateUser, AppPermissions.EditUser, AppPermissions.DeleteUser,
                AppPermissions.ManageRoles, AppPermissions.SystemAdmin
            },
            [AppRoles.Manager] = new[]
            {
                AppPermissions.ViewCustomers, AppPermissions.CreateCustomer, AppPermissions.EditCustomer,
                AppPermissions.ViewUsers
            },
            [AppRoles.User] = new[]
            {
                AppPermissions.ViewCustomers, AppPermissions.CreateCustomer
            },
            [AppRoles.Guest] = new[]
            {
                AppPermissions.ViewCustomers
            }
        };

        public static bool HasPermission(string role, string permission)
        {
            return RolePermissionMap.TryGetValue(role, out var permissions) && 
                   permissions.Contains(permission);
        }
    }
} 