using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlayOn.Common.ApplicationConstants;
using PlayOn.Data.Configurations.SeedData;
using PlayOn.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlayOn.Data.Configurations
{
    public class ApplicationRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        {
            builder.HasData(
                        new ApplicationRole
                        {
                            Id = Guid.Parse(SeedRoleIds.AdministratorRole),
                            Name = RoleNames.Administrator,
                            NormalizedName = RoleNames.Administrator.ToUpperInvariant(),
                            ConcurrencyStamp = "22fd9079-6192-4b3b-a553-98a4812f7869"
                        },
                        new ApplicationRole
                        {
                            Id = Guid.Parse(SeedRoleIds.UserRole),
                            Name = RoleNames.User,
                            NormalizedName = RoleNames.User.ToUpperInvariant(),
                            ConcurrencyStamp = "d6ecc739-d33f-4f53-8dfc-feac22c08b9d"
                        }
                    );
        }
    }
}
