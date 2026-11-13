using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PlayOn.Data.Configurations;
using PlayOn.Data.Models;

namespace PlayOn.Data
{
    public class PlayOnDbContext(DbContextOptions<PlayOnDbContext> options)
        : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>(options)
    {

        public virtual DbSet<Sport> Sports { get; set; } = null!;
        public virtual DbSet<SportPreference> SportPreferences { get; set; } = null!;
        public virtual DbSet<Venue> Venues { get; set; } = null!;
        public virtual DbSet<Court> Courts { get; set; } = null!;
        public virtual DbSet<CourtSport> CourtSports { get; set; } = null!;
        public virtual DbSet<Match> Matches { get; set; } = null!;
        public virtual DbSet<MatchParticipant> MatchParticipants { get; set; } = null!;
        public virtual DbSet<PlayerRating> PlayerRatings { get; set; } = null!;
        public virtual DbSet<Notification> Notifications { get; set; } = null!;
        public virtual DbSet<Event> Events { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ApplicationUserConfiguration());
            modelBuilder.ApplyConfiguration(new CourtConfiguration());
            modelBuilder.ApplyConfiguration(new CourtSportConfiguration());
            modelBuilder.ApplyConfiguration(new EventConfiguration());
            modelBuilder.ApplyConfiguration(new MatchConfiguration());
            modelBuilder.ApplyConfiguration(new MatchParticipantConfiguration());
            modelBuilder.ApplyConfiguration(new NotificationConfiguration());
            modelBuilder.ApplyConfiguration(new PlayerRatingConfiguration());
            modelBuilder.ApplyConfiguration(new SportConfiguration());
            modelBuilder.ApplyConfiguration(new SportPreferenceConfiguration());
            modelBuilder.ApplyConfiguration(new VenueConfiguration());

        }

    }
}
