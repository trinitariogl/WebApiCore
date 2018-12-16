
namespace DataServiceLayer.Context
{
    using DataServiceInterfaces.Models;
    using Microsoft.EntityFrameworkCore;

    public partial class SecurityModelContext : DbContext
    {
        public SecurityModelContext()
        {
        }

        public SecurityModelContext(DbContextOptions<SecurityModelContext> options): base(options)
        {
        }

        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<UserAccounts> UserAccounts { get; set; }
        //public DbSet<UserRoles> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRoles>()
                .HasKey(t => new { t.UserId, t.RoleId });

            modelBuilder.Entity<UserRoles>()
                .HasOne(pt => pt.User)
                .WithMany(p => p.UserRoles)
                .HasForeignKey(pt => pt.UserId);

            modelBuilder.Entity<UserRoles>()
                .HasOne(pt => pt.Rol)
                .WithMany(t => t.UserRoles)
                .HasForeignKey(pt => pt.RoleId);
        }
    }
}
