namespace RBAC_API_project.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public virtual ICollection<UserRole>? UserRoles { get; set; }

        public User(string Name, string Email, string Password)
        {
            this.Name = Name;
            this.Email = Email;
            this.Password = BCrypt.Net.BCrypt.HashPassword(Password);
        }
    }
}
