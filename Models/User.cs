using Microsoft.AspNetCore.Identity;

namespace RBAC_API_project.Models
{
    public class User :IdentityUser
    {
        public string Name { get; set; }
        public DateTime ExpiresOn;
        public bool IsAuthenticated ;
        public List<string> Roles;
        public string Token;
    }
}
