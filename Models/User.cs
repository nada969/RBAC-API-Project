namespace RBAC_API_project.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public virtual ICollection<UserRole>? UserRoles { get; set; }
    }
}
