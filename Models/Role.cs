namespace RBAC_API_project.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string RoleName { get; set; }

        public virtual ICollection<UserRole>? UserRoles { get; set; }
    }
}
