using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PlayOn.Data.Configurations;
using PlayOn.Data.Models;

namespace PlayOn.Data
{
    /// <summary>
    /// The Entity Framework Core database context of the PlayOn application.
    /// Extends the ASP.NET Core Identity context with the application's domain entities.
    /// </summary>
    /// <param name="options">The options used to configure the context.</param>
    public class PlayOnDbContext(DbContextOptions<PlayOnDbContext> options)
        : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>(options)
    {
        /// <summary>
        /// Sports that can be played on the platform.
        /// </summary>
        public virtual DbSet<Sport> Sports { get; set; } = null!;

        /// <summary>
        /// Users' preferred sports and their skill levels.
        /// </summary>
        public virtual DbSet<SportPreference> SportPreferences { get; set; } = null!;

        /// <summary>
        /// Venues (sports facilities) where events take place.
        /// </summary>
        public virtual DbSet<Venue> Venues { get; set; } = null!;

        /// <summary>
        /// Courts available for booking.
        /// </summary>
        public virtual DbSet<Court> Courts { get; set; } = null!;

        /// <summary>
        /// Mappings between courts and the sports that can be played on them.
        /// </summary>
        public virtual DbSet<CourtSport> CourtSports { get; set; } = null!;

        /// <summary>
        /// Matches organised by members.
        /// </summary>
        public virtual DbSet<Match> Matches { get; set; } = null!;

        /// <summary>
        /// Users' participation in matches.
        /// </summary>
        public virtual DbSet<MatchParticipant> MatchParticipants { get; set; } = null!;

        /// <summary>
        /// Ratings players give each other after a match.
        /// </summary>
        public virtual DbSet<PlayerRating> PlayerRatings { get; set; } = null!;

        /// <summary>
        /// Notifications sent to users.
        /// </summary>
        public virtual DbSet<Notification> Notifications { get; set; } = null!;

        /// <summary>
        /// Scheduled events linking a match to a sport, court and venue.
        /// </summary>
        public virtual DbSet<Event> Events { get; set; } = null!;

        /// <summary>
        /// Configures the entity model by applying the Identity defaults
        /// and all of the application's entity type configurations.
        /// </summary>
        /// <param name="modelBuilder">The builder used to construct the model for this context.</param>
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

