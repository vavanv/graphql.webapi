namespace GraphQL.WebApi.Mvc.Models
{
    public static class AppRoles
    {
        public const string Admin = "Admin";
        public const string Manager = "Manager";
        public const string User = "User";
        public const string Guest = "Guest";

        public static readonly string[] AllRoles = { Admin, Manager, User, Guest };
    }
}