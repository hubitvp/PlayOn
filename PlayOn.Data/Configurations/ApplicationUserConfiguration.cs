
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlayOn.Data.Models;

namespace PlayOn.Data.Configurations
{
    /// <summary>
    /// Entity Framework Core configuration for the <see cref="ApplicationUser"/> entity.
    /// </summary>
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        /// <summary>
        /// Configures the <see cref="ApplicationUser"/> entity: enforces a unique index on the normalized email address so two users cannot share the same email.
        /// </summary>
        /// <param name="builder">The builder used to configure the <see cref="ApplicationUser"/> entity type.</param>
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasIndex(x => x.NormalizedEmail).IsUnique();
        }
    }
}
