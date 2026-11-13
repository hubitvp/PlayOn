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
                    UserId = Guid.Parse(SeedUserIds.User001),
                    SportId = Guid.Parse(SeedSportIds.Football),
                    SkillLevel = SkillLevel.Intermediate
                },
                new SportPreference
                {
                    UserId = Guid.Parse(SeedUserIds.User002),
                    SportId = Guid.Parse(SeedSportIds.Basketball),
                    SkillLevel = SkillLevel.Advanced
                },
                new SportPreference
                {
                    UserId = Guid.Parse(SeedUserIds.User003),
                    SportId = Guid.Parse(SeedSportIds.Tennis),
                    SkillLevel = SkillLevel.Beginner
                },
                new SportPreference
                {
                    UserId = Guid.Parse(SeedUserIds.User004),
                    SportId = Guid.Parse(SeedSportIds.Volleyball),
                    SkillLevel = SkillLevel.Intermediate
                },
                new SportPreference
                {
                    UserId = Guid.Parse(SeedUserIds.User005),
                    SportId = Guid.Parse(SeedSportIds.Padel),
                    SkillLevel = SkillLevel.Advanced
                },
                new SportPreference
                {
                    UserId = Guid.Parse(SeedUserIds.User006),
                    SportId = Guid.Parse(SeedSportIds.Badminton),
                    SkillLevel = SkillLevel.Beginner
                },
                new SportPreference
                {
                    UserId = Guid.Parse(SeedUserIds.User007),
                    SportId = Guid.Parse(SeedSportIds.Handball),
                    SkillLevel = SkillLevel.Intermediate
                },
                new SportPreference
                {
                    UserId = Guid.Parse(SeedUserIds.User008),
                    SportId = Guid.Parse(SeedSportIds.Rugby),
                    SkillLevel = SkillLevel.Beginner
                },
                new SportPreference
                {
                    UserId = Guid.Parse(SeedUserIds.User009),
                    SportId = Guid.Parse(SeedSportIds.Hockey),
                    SkillLevel = SkillLevel.Advanced
                },
                new SportPreference
                {
                    UserId = Guid.Parse(SeedUserIds.User010),
                    SportId = Guid.Parse(SeedSportIds.Baseball),
                    SkillLevel = SkillLevel.Intermediate
                },
                new SportPreference
                {
                    UserId = Guid.Parse(SeedUserIds.User001),
                    SportId = Guid.Parse(SeedSportIds.Basketball),
                    SkillLevel = SkillLevel.Intermediate
                },
                new SportPreference
                {
                    UserId = Guid.Parse(SeedUserIds.User004),
                    SportId = Guid.Parse(SeedSportIds.Basketball),
                    SkillLevel = SkillLevel.Intermediate
                },
                new SportPreference
                {
                    UserId = Guid.Parse(SeedUserIds.User005),
                    SportId = Guid.Parse(SeedSportIds.Basketball),
                    SkillLevel = SkillLevel.Intermediate
                },
                new SportPreference
                {
                    UserId = Guid.Parse(SeedUserIds.User001),
                    SportId = Guid.Parse(SeedSportIds.Padel),
                    SkillLevel = SkillLevel.Intermediate
                },
                new SportPreference
                {
                    UserId = Guid.Parse(SeedUserIds.User002),
                    SportId = Guid.Parse(SeedSportIds.Padel),
                    SkillLevel = SkillLevel.Intermediate
                },
                new SportPreference
                {
                    UserId = Guid.Parse(SeedUserIds.User004),
                    SportId = Guid.Parse(SeedSportIds.Padel),
                    SkillLevel = SkillLevel.Intermediate
                },
                new SportPreference
                {
                    UserId = Guid.Parse(SeedUserIds.User009),
                    SportId = Guid.Parse(SeedSportIds.Padel),
                    SkillLevel = SkillLevel.Advanced
                },
                new SportPreference
                {
                    UserId = Guid.Parse(SeedUserIds.User009),
                    SportId = Guid.Parse(SeedSportIds.Handball),
                    SkillLevel = SkillLevel.Intermediate
                },
                new SportPreference
                {
                    UserId = Guid.Parse(SeedUserIds.User010),
                    SportId = Guid.Parse(SeedSportIds.Handball),
                    SkillLevel = SkillLevel.Intermediate
                });
        }
    }
}
