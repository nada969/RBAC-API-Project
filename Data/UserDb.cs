using Microsoft.EntityFrameworkCore;
using RBAC_API_project.Models;

namespace RBAC_API_project.Data
{
    
    public class UserDb:DbContext
    {
        public DbSet<User> users { get; set; }
        public DbSet<Role> roles { get; set; }
        public DbSet<UserRole> userRoles { get; set; }

        public UserDb(DbContextOptions<UserDb> options)
            :base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Composite Primary Key for UserRole
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new {ur.UserId , ur.RoleId} );

            // User -> UserRoles
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            // Role -> UserRoles
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

        }
    }
}
