using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlayOn.Data.Models;

namespace PlayOn.Data.Configurations
{
    /// <summary>
    /// Entity Framework Core configuration for the <see cref="MatchParticipant"/> entity.
    /// </summary>
    public class MatchParticipantConfiguration : IEntityTypeConfiguration<MatchParticipant>
    {
        /// <summary>
        /// Configures the <see cref="MatchParticipant"/> entity: defines the composite primary key (match and user) and the relationships to <see cref="Match"/> and <see cref="ApplicationUser"/>, using restricted delete behavior.
        /// </summary>
        /// <param name="builder">The builder used to configure the <see cref="MatchParticipant"/> entity type.</param>
        public void Configure(EntityTypeBuilder<MatchParticipant> builder)
        {

            builder.HasKey(x => new
            {
                x.MatchId,
                x.UserId
            });


            builder.HasOne(x => x.Match)
                .WithMany(x => x.Participants)
                .HasForeignKey(x => x.MatchId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
