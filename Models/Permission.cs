namespace RBAC_API_project.Models
{
    public class Permission
    {
        public int Id { get; set; }
        public string PermissionName { get; set; }

        public virtual ICollection<RolePermission>? RolePermissions { get; set; }
    }
}
