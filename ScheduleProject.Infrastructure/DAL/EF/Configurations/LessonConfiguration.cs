using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScheduleProject.Core.Entities;
using ScheduleProject.Core.Entities.Enums;
using ScheduleProject.Core.Entities.ValueObjects;

namespace ScheduleProject.Infrastructure.DAL.EF.Configurations;

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

		builder.Property(x => x.LessonType)
			.IsRequired()
			.HasMaxLength(LessonType.MaxLength)
			.HasConversion(
				x => x.Name,
				y => LessonType.Create(y).Value
			);

		builder.Property(x => x.StartTime).IsRequired();
		builder.Property(x => x.EndTime).IsRequired();

		builder.Property(x => x.ScheduleWeeksType)
			.IsRequired()
			.HasMaxLength(ScheduleWeeksType.MaxLength)
			.HasConversion(
				x => x.Name,
				y => ScheduleWeeksType.FromName(y)! 
			);

		builder.Property(x => x.Day)
			.IsRequired()
			.HasMaxLength(Day.MaxLength)
			.HasConversion(
				x => x.Name,
				y => Day.FromName(y)!
			);

		builder.HasOne(x => x.Schedule)
			.WithMany(x => x.Lessons)
			.HasForeignKey(x => x.ScheduleId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}
