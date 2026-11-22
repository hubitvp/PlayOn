using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlayOn.Data.Models;

namespace PlayOn.Data.Configurations
{
    public class CourtSportConfiguration : IEntityTypeConfiguration<CourtSport>
    {
        public void Configure(EntityTypeBuilder<CourtSport> builder)
        {
            builder.HasKey(x => new { x.CourtId, x.SportId });

            builder.HasOne(x => x.Court)
                .WithMany(x => x.Sports)
                .HasForeignKey(x => x.CourtId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Sport)
                .WithMany(x => x.Courts)
                .HasForeignKey(x => x.SportId)
                .OnDelete(DeleteBehavior.Restrict);


        }
    }
}
