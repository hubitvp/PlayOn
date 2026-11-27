using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlayOn.Data.Models;

namespace PlayOn.Data.Configurations
{
    /// <summary>
    /// Entity Framework Core configuration for the <see cref="SportPreference"/> entity.
    /// </summary>
    public class SportPreferenceConfiguration : IEntityTypeConfiguration<SportPreference>
    {
        /// <summary>
        /// Configures the <see cref="SportPreference"/> entity: defines the composite primary key (user and sport) and the relationships to <see cref="ApplicationUser"/> and <see cref="Sport"/>, using restricted delete behavior.
        /// </summary>
        /// <param name="builder">The builder used to configure the <see cref="SportPreference"/> entity type.</param>
        public void Configure(EntityTypeBuilder<SportPreference> builder)
        {
            builder.HasKey(x => new { x.UserId, x.SportId });

            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Sport)
                .WithMany(x => x.Preferences)
                .HasForeignKey(x => x.SportId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
