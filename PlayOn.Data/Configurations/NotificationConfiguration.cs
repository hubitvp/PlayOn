using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlayOn.Data.Models;

namespace PlayOn.Data.Configurations
{
    /// <summary>
    /// Entity Framework Core configuration for the <see cref="Notification"/> entity.
    /// </summary>
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        /// <summary>
        /// Configures the <see cref="Notification"/> entity: defines the relationships to the receiving <see cref="ApplicationUser"/> and the optional related <see cref="Match"/>, using restricted delete behavior.
        /// </summary>
        /// <param name="builder">The builder used to configure the <see cref="Notification"/> entity type.</param>
        public void Configure(EntityTypeBuilder<Notification> builder)
        {

            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Match)
                .WithMany()
                .HasForeignKey(x => x.MatchId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
