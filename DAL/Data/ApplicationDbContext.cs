using Core.Enums;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        Database.EnsureDeleted();
        Database.EnsureCreated();
    }
    public DbSet<UserModel> Users { get; set; }
    public DbSet<RoleModel> Roles { get; set; }
    public DbSet<RoleUserModel> RoleUser { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RoleUserModel>()
            .Property(c => c.RoleId)
            .HasConversion<int>();

        modelBuilder.Entity<UserModel>()
           .HasMany(e => e.Roles)
           .WithMany(e => e.Users)
           .UsingEntity<RoleUserModel>(
                l => l.HasOne<RoleModel>().WithMany().HasForeignKey(e => e.RoleId),
                r => r.HasOne<UserModel>().WithMany().HasForeignKey(e => e.UserId));
        modelBuilder.Entity<RoleModel>()
            .Property(c => c.Id)
            .HasConversion<int>();

        Random random = new Random();
        var roles = new RoleModel[]
        {
            new RoleModel {Id = RoleType.User},
            new RoleModel {Id = RoleType.Admin},
            new RoleModel {Id = RoleType.Support},
            new RoleModel {Id = RoleType.SuperAdmin}
        };

        modelBuilder.Entity<RoleModel>().HasData(roles);

        var users = Enumerable.Range(1, 20).Select(i => new UserModel
        {
            Id = i,
            Email = $"user{i}@test.com",
            Age = random.Next(18, 101),
            Name = $"User {i}",
        }).ToArray();

        modelBuilder.Entity<UserModel>().HasData(users);

        var userRoles = new List<RoleUserModel>();
        foreach (var user in users)
        {
            var userRolesToAdd = roles.OrderBy(_ => random.Next()).Take(random.Next(1, 3)).ToList();
            foreach (var role in userRolesToAdd)
            {
                userRoles.Add(new RoleUserModel { UserId = user.Id, RoleId = role.Id });
            }
        }

        modelBuilder.Entity<RoleUserModel>().HasData(userRoles);
    }
}
