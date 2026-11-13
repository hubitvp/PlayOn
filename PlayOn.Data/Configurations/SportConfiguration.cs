using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlayOn.Common.Enums;
using PlayOn.Data.Configurations.SeedData;
using PlayOn.Data.Models;

namespace PlayOn.Data.Configurations
{
    public class SportConfiguration : IEntityTypeConfiguration<Sport>
    {
        public void Configure(EntityTypeBuilder<Sport> builder)
        {
            builder.HasIndex(x => x.Name)
                .IsUnique();

            builder.HasData(
                new Sport
                {
                    Id = Guid.Parse(SeedSportIds.Football),
                    Name = "Football",
                    Icon = SportIcon.Football,
                    DefaultMinPlayers = 6,
                    DefaultMaxPlayers = 22
                },
                new Sport
                {
                    Id = Guid.Parse(SeedSportIds.Basketball),
                    Name = "Basketball",
                    Icon = SportIcon.Basketball,
                    DefaultMinPlayers = 4,
                    DefaultMaxPlayers = 10
                },
                new Sport
                {
                    Id = Guid.Parse(SeedSportIds.Tennis),
                    Name = "Tennis",
                    Icon = SportIcon.TableTennisPaddle,
                    DefaultMinPlayers = 2,
                    DefaultMaxPlayers = 4
                },
                new Sport
                {
                    Id = Guid.Parse(SeedSportIds.Volleyball),
                    Name = "Volleyball",
                    Icon = SportIcon.Volleyball,
                    DefaultMinPlayers = 4,
                    DefaultMaxPlayers = 12
                },
                new Sport
                {
                    Id = Guid.Parse(SeedSportIds.Padel),
                    Name = "Padel",
                    Icon = SportIcon.TableTennisPaddle,
                    DefaultMinPlayers = 2,
                    DefaultMaxPlayers = 4
                },
                new Sport
                {
                    Id = Guid.Parse(SeedSportIds.Badminton),
                    Name = "Badminton",
                    Icon = SportIcon.Badminton,
                    DefaultMinPlayers = 2,
                    DefaultMaxPlayers = 4
                },
                new Sport
                {
                    Id = Guid.Parse(SeedSportIds.Handball),
                    Name = "Handball",
                    Icon = SportIcon.Handball,
                    DefaultMinPlayers = 6,
                    DefaultMaxPlayers = 14
                },
                new Sport
                {
                    Id = Guid.Parse(SeedSportIds.Rugby),
                    Name = "Rugby",
                    Icon = SportIcon.Rugby,
                    DefaultMinPlayers = 10,
                    DefaultMaxPlayers = 30
                },
                new Sport
                {
                    Id = Guid.Parse(SeedSportIds.Hockey),
                    Name = "Hockey",
                    Icon = SportIcon.HockeyPuck,
                    DefaultMinPlayers = 6,
                    DefaultMaxPlayers = 22
                },
                new Sport
                {
                    Id = Guid.Parse(SeedSportIds.Baseball),
                    Name = "Baseball",
                    Icon = SportIcon.Baseball,
                    DefaultMinPlayers = 6,
                    DefaultMaxPlayers = 18
                });
        }
    }
}
