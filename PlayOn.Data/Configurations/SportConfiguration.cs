using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
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
                new Sport { Id = 1, Name = "Football", IconClass = "fa-solid fa-futbol", DefaultMinPlayers = 6, DefaultMaxPlayers = 22 },
                new Sport { Id = 2, Name = "Basketball", IconClass = "fa-solid fa-basketball", DefaultMinPlayers = 4, DefaultMaxPlayers = 10 },
                new Sport { Id = 3, Name = "Tennis", IconClass = "fa-solid fa-tennis-ball", DefaultMinPlayers = 2, DefaultMaxPlayers = 4 },
                new Sport { Id = 4, Name = "Volleyball", IconClass = "fa-solid fa-volleyball", DefaultMinPlayers = 4, DefaultMaxPlayers = 12 },
                new Sport { Id = 5, Name = "Padel", IconClass = "fa-solid fa-table-tennis-paddle-ball", DefaultMinPlayers = 2, DefaultMaxPlayers = 4 },
                new Sport { Id = 6, Name = "Badminton", IconClass = "fa-solid fa-feather", DefaultMinPlayers = 2, DefaultMaxPlayers = 4 },
                new Sport { Id = 7, Name = "Handball", IconClass = "fa-solid fa-hand", DefaultMinPlayers = 6, DefaultMaxPlayers = 14 },
                new Sport { Id = 8, Name = "Rugby", IconClass = "fa-solid fa-football", DefaultMinPlayers = 10, DefaultMaxPlayers = 30 },
                new Sport { Id = 9, Name = "Hockey", IconClass = "fa-solid fa-hockey-puck", DefaultMinPlayers = 6, DefaultMaxPlayers = 22 },
                new Sport { Id = 10, Name = "Baseball", IconClass = "fa-solid fa-baseball", DefaultMinPlayers = 6, DefaultMaxPlayers = 18 });
        }
    }
}
