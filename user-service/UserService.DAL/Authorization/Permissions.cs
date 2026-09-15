namespace UserService.DAL.Authorization
{
    public static class Permissions
    {
        //Account permissions
        public const string AccountReadAll = "account.read_all";
        public const string AccountRead = "account.read";
        public const string AccountDelete = "account.delete";
        public const string AccountUpdate = "account.update";
        public const string AccountCreate = "account.create";
        public const string AccountRoleAssign = "account_role.assign";
        public const string AccountRoleRemove = "account_role.remove";
        public const string AccountRoleRead = "account_role.read";
        //Role permissions
        public const string RoleReadAll = "role.read_all";
        public const string RoleCreate = "role.create";
        public const string RoleRead = "role.read";
        public const string RoleUpdate = "role.update";
        public const string RoleDelete = "role.delete";
        public const string RolePermissionAssign = "role_permission.assign";
        public const string RolePermissionRemove = "role_permission.remove";
        public const string RolePermissionRead = "role_permission.read";
        //Permission permissions
        public const string PermissionReadAll = "permission.read_all";
        public const string PermissionCreate = "permission.create";
        public const string PermissionRead = "permission.read";
        public const string PermissionUpdate = "permission.update";
        public const string PermissionDelete = "permission.delete";
    }
}
