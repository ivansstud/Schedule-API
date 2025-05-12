using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScheduleProject.Core.Entities;
using ScheduleProject.Core.Entities.Enums;
using ScheduleProject.Core.Entities.ValueObjects;

namespace ScheduleProject.Infrastructure.DAL.EF.Configurations;

class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
{
	public void Configure(EntityTypeBuilder<Schedule> builder)
	{
		builder.HasKey(x => x.Id);
		builder.Property(x => x.Id).ValueGeneratedOnAdd();

		builder.Property(x => x.Name).HasMaxLength(Schedule.MaxNameLength);
		builder.Property(x => x.Description).HasMaxLength(Schedule.MaxDescriptionLength);

		builder.Property(x => x.Type)
			.IsRequired()
			.HasMaxLength(ScheduleType.MaxLength)
			.HasConversion(
				x => x.Name,
				y => ScheduleType.FromName(y)!
			);

		builder.Property(x => x.WeeksType)
			.IsRequired()
			.HasMaxLength(ScheduleWeeksType.MaxLength)
			.HasConversion(
				x => x.Name,
				y => ScheduleWeeksType.FromName(y)!
			);

		builder.HasOne(x => x.Institution)
			.WithMany(x => x.Schedules)
			.HasForeignKey(x => x.InstitutionId);

		builder.HasMany(x => x.Members)
			.WithOne(x => x.Schedule)
			.HasForeignKey(x => x.ScheduleId);

		builder.HasMany(x => x.Lessons)
			.WithOne(x => x.Schedule)
			.HasForeignKey(x => x.ScheduleId);

		builder.Navigation(p => p.Members).UsePropertyAccessMode(PropertyAccessMode.Field);
		builder.Navigation(p => p.Lessons).UsePropertyAccessMode(PropertyAccessMode.Field);
	}
}
