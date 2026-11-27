using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlayOn.Data.Models;

namespace PlayOn.Data.Configurations
{
    /// <summary>
    /// Entity Framework Core configuration for the <see cref="Sport"/> entity.
    /// </summary>
    public class SportConfiguration : IEntityTypeConfiguration<Sport>
    {
        /// <summary>
        /// Configures the <see cref="Sport"/> entity: enforces a unique index on the sport name.
        /// </summary>
        /// <param name="builder">The builder used to configure the <see cref="Sport"/> entity type.</param>
        public void Configure(EntityTypeBuilder<Sport> builder)
        {
            builder.HasIndex(x => x.Name)
                .IsUnique();

        }
    }
}
