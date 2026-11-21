using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlayOn.Data.Configurations.SeedData;

namespace PlayOn.Data.Configurations
{
    public class ApplicationUserRoleConfiguration
        : IEntityTypeConfiguration<IdentityUserRole<Guid>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<Guid>> builder)
        {
            // Regular users
            builder.HasData(
                new IdentityUserRole<Guid>
                {
                    UserId = Guid.Parse(SeedUserIds.User001),
                    RoleId = Guid.Parse(SeedRoleIds.UserRole)
                },
                new IdentityUserRole<Guid>
                {
                    UserId = Guid.Parse(SeedUserIds.User002),
                    RoleId = Guid.Parse(SeedRoleIds.UserRole)
                },
                new IdentityUserRole<Guid>
                {
                    UserId = Guid.Parse(SeedUserIds.User003),
                    RoleId = Guid.Parse(SeedRoleIds.UserRole)
                },
                new IdentityUserRole<Guid>
                {
                    UserId = Guid.Parse(SeedUserIds.User004),
                    RoleId = Guid.Parse(SeedRoleIds.UserRole)
                },
                new IdentityUserRole<Guid>
                {
                    UserId = Guid.Parse(SeedUserIds.User005),
                    RoleId = Guid.Parse(SeedRoleIds.UserRole)
                },
                new IdentityUserRole<Guid>
                {
                    UserId = Guid.Parse(SeedUserIds.User006),
                    RoleId = Guid.Parse(SeedRoleIds.UserRole)
                },
                new IdentityUserRole<Guid>
                {
                    UserId = Guid.Parse(SeedUserIds.User007),
                    RoleId = Guid.Parse(SeedRoleIds.UserRole)
                },
                new IdentityUserRole<Guid>
                {
                    UserId = Guid.Parse(SeedUserIds.User008),
                    RoleId = Guid.Parse(SeedRoleIds.UserRole)
                },
                new IdentityUserRole<Guid>
                {
                    UserId = Guid.Parse(SeedUserIds.User009),
                    RoleId = Guid.Parse(SeedRoleIds.UserRole)
                },
                new IdentityUserRole<Guid>
                {
                    UserId = Guid.Parse(SeedUserIds.User010),
                    RoleId = Guid.Parse(SeedRoleIds.UserRole)
                }
            );

            // Administrator users
            builder.HasData(
                new IdentityUserRole<Guid>
                {
                    UserId = Guid.Parse(SeedUserIds.Admin001),
                    RoleId = Guid.Parse(SeedRoleIds.AdministratorRole)
                },
                new IdentityUserRole<Guid>
                {
                    UserId = Guid.Parse(SeedUserIds.Admin002),
                    RoleId = Guid.Parse(SeedRoleIds.AdministratorRole)
                },
                new IdentityUserRole<Guid>
                {
                    UserId = Guid.Parse(SeedUserIds.Admin003),
                    RoleId = Guid.Parse(SeedRoleIds.AdministratorRole)
                }
            );
        }
    }
}