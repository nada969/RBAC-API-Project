using System.ComponentModel.DataAnnotations.Schema;

namespace RBAC_API_project.Models
{
    public class RolePermission
    {
        [ForeignKey("Role")]
        public int RoleId { get; set; }
        public virtual Role? Role { get; set; }
        [ForeignKey("Permission")]
        public int PermissionId { get; set; }
        public virtual Permission? Permission { get; set; }
    }
}
