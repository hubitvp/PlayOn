using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlayOn.Data.Models;

namespace PlayOn.Data.Configurations
{
    public class SportPreferenceConfiguration : IEntityTypeConfiguration<SportPreference>
    {
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
