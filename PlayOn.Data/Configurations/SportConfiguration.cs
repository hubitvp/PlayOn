using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlayOn.Common.Enums;
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
                    Id = 1,
                    Name = "Football",
                    Icon = SportIcon.Football,
                    DefaultMinPlayers = 6,
                    DefaultMaxPlayers = 22
                },
                new Sport
                {
                    Id = 2,
                    Name = "Basketball",
                    Icon = SportIcon.Basketball,
                    DefaultMinPlayers = 4,
                    DefaultMaxPlayers = 10
                },
                new Sport
                {
                    Id = 3,
                    Name = "Tennis",
                    Icon = SportIcon.TableTennisPaddle,
                    DefaultMinPlayers = 2,
                    DefaultMaxPlayers = 4
                },
                new Sport
                {
                    Id = 4,
                    Name = "Volleyball",
                    Icon = SportIcon.Volleyball,
                    DefaultMinPlayers = 4,
                    DefaultMaxPlayers = 12
                },
                new Sport
                {
                    Id = 5,
                    Name = "Padel",
                    Icon = SportIcon.TableTennisPaddle,
                    DefaultMinPlayers = 2,
                    DefaultMaxPlayers = 4
                },
                new Sport
                {
                    Id = 6,
                    Name = "Badminton",
                    Icon = SportIcon.Badminton,
                    DefaultMinPlayers = 2,
                    DefaultMaxPlayers = 4
                },
                new Sport
                {
                    Id = 7,
                    Name = "Handball",
                    Icon = SportIcon.Handball,
                    DefaultMinPlayers = 6,
                    DefaultMaxPlayers = 14
                },
                new Sport
                {
                    Id = 8,
                    Name = "Rugby",
                    Icon = SportIcon.Rugby,
                    DefaultMinPlayers = 10,
                    DefaultMaxPlayers = 30
                },
                new Sport
                {
                    Id = 9,
                    Name = "Hockey",
                    Icon = SportIcon.HockeyPuck,
                    DefaultMinPlayers = 6,
                    DefaultMaxPlayers = 22
                },
                new Sport
                {
                    Id = 10,
                    Name = "Baseball",
                    Icon = SportIcon.Baseball,
                    DefaultMinPlayers = 6,
                    DefaultMaxPlayers = 18
                });
        }
    }
}
