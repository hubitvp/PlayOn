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



            builder.HasData(
                new CourtSport { CourtId = 1, SportId = 1 },
                new CourtSport { CourtId = 2, SportId = 2 },
                new CourtSport { CourtId = 3, SportId = 3 },
                new CourtSport { CourtId = 4, SportId = 4 },
                new CourtSport { CourtId = 5, SportId = 5 },
                new CourtSport { CourtId = 6, SportId = 6 },
                new CourtSport { CourtId = 7, SportId = 7 },
                new CourtSport { CourtId = 8, SportId = 8 },
                new CourtSport { CourtId = 9, SportId = 9 },
                new CourtSport { CourtId = 10, SportId = 10 },
                new CourtSport { CourtId = 2, SportId = 4 },
                new CourtSport { CourtId = 2, SportId = 7 },
                new CourtSport { CourtId = 7, SportId = 2 },
                new CourtSport { CourtId = 1, SportId = 9 });
        }
    }
}
