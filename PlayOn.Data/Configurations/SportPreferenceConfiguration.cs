using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlayOn.Common.Enums;
using PlayOn.Data.Configurations.SeedData;
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


            builder.HasData(
                new SportPreference
                {
                    UserId = SeedUserIds.User001,
                    SportId = 1,
                    SkillLevel = SkillLevel.Intermediate
                },
                new SportPreference
                {
                    UserId = SeedUserIds.User002,
                    SportId = 2,
                    SkillLevel = SkillLevel.Advanced
                },
                new SportPreference
                {
                    UserId = SeedUserIds.User003,
                    SportId = 3,
                    SkillLevel = SkillLevel.Beginner
                },
                new SportPreference
                {
                    UserId = SeedUserIds.User004,
                    SportId = 4,
                    SkillLevel = SkillLevel.Intermediate
                },
                new SportPreference
                {
                    UserId = SeedUserIds.User005,
                    SportId = 5,
                    SkillLevel = SkillLevel.Advanced
                },
                new SportPreference
                {
                    UserId = SeedUserIds.User006,
                    SportId = 6,
                    SkillLevel = SkillLevel.Beginner
                },
                new SportPreference
                {
                    UserId = SeedUserIds.User007,
                    SportId = 7,
                    SkillLevel = SkillLevel.Intermediate
                },
                new SportPreference
                {
                    UserId = SeedUserIds.User008,
                    SportId = 8,
                    SkillLevel = SkillLevel.Beginner
                },
                new SportPreference
                {
                    UserId = SeedUserIds.User009,
                    SportId = 9,
                    SkillLevel = SkillLevel.Advanced
                },
                new SportPreference
                {
                    UserId = SeedUserIds.User010,
                    SportId = 10,
                    SkillLevel = SkillLevel.Intermediate
                },
                new SportPreference
                {
                    UserId = SeedUserIds.User001,
                    SportId = 2,
                    SkillLevel = SkillLevel.Intermediate
                },
                new SportPreference
                {
                    UserId = SeedUserIds.User004,
                    SportId = 2,
                    SkillLevel = SkillLevel.Intermediate
                },
                new SportPreference
                {
                    UserId = SeedUserIds.User005,
                    SportId = 2,
                    SkillLevel = SkillLevel.Intermediate
                },
                new SportPreference
                {
                    UserId = SeedUserIds.User001,
                    SportId = 5,
                    SkillLevel = SkillLevel.Intermediate
                },
                new SportPreference
                {
                    UserId = SeedUserIds.User002,
                    SportId = 5,
                    SkillLevel = SkillLevel.Intermediate
                },
                new SportPreference
                {
                    UserId = SeedUserIds.User004,
                    SportId = 5,
                    SkillLevel = SkillLevel.Intermediate
                },
                new SportPreference
                {
                    UserId = SeedUserIds.User009,
                    SportId = 5,
                    SkillLevel = SkillLevel.Advanced
                },
                new SportPreference
                {
                    UserId = SeedUserIds.User009,
                    SportId = 7,
                    SkillLevel = SkillLevel.Intermediate
                },
                new SportPreference
                {
                    UserId = SeedUserIds.User010,
                    SportId = 7,
                    SkillLevel = SkillLevel.Intermediate
                });
        }
    }
}
