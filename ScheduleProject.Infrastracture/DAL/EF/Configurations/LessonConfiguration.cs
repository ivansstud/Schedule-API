using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScheduleProject.Core.Entities;
using ScheduleProject.Core.Entities.Enums;
using ScheduleProject.Core.Entities.ValueObjects;

namespace ScheduleProject.Infrastracture.DAL.EF.Configurations;

class LessonConfiguration : IEntityTypeConfiguration<Lesson>
{
	public void Configure(EntityTypeBuilder<Lesson> builder)
	{
		builder.HasKey(x => x.Id);
		builder.Property(x => x.Id).ValueGeneratedOnAdd();

		builder.Property(x => x.Name).IsRequired().HasMaxLength(Lesson.MaxNameLength);
		builder.Property(x => x.Audience).HasMaxLength(Lesson.MaxAudienceLength);
		builder.Property(x => x.Description).HasMaxLength(Lesson.MaxDescriptionLength);
		builder.Property(x => x.TeacherName).HasMaxLength(Lesson.MaxTeacherNameLength);

		builder.OwnsOne(x => x.LessonType, b =>
		{
			b.Property(x => x.Value)
				.HasColumnName(nameof(LessonType))
				.HasMaxLength(LessonType.MaxValueLength)
				.IsRequired();
		});

		builder.Property(x => x.StartTime).IsRequired();
		builder.Property(x => x.EndTime).IsRequired();

		builder.Property(x => x.SheduleWeeksType)
			.IsRequired()
			.HasConversion(
				x => x.ToString(),
				y => Enum.Parse<ScheduleWeeksType>(y)
			);
		builder.Property(x => x.DayOfWeek)
			.IsRequired()
			.HasConversion(
				x => x.ToString(),
				y => Enum.Parse<DayOfWeek>(y)
			);

		builder.HasOne(x => x.Schedule)
			.WithMany(x => x.Lessons)
			.HasForeignKey(x => x.ScheduleId);
	}
}
