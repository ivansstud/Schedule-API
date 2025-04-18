﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScheduleProject.Core.Entities;
using ScheduleProject.Core.Entities.Enums;

namespace ScheduleProject.Infrastracture.EF.Configurations;

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
			.HasConversion(
				x => x.ToString(), // Преобразование Enum -> String
				y => Enum.Parse<ScheduleType>(y) // Преобразование String -> Enum
			);

		builder.Property(x => x.WeeksType)
			.IsRequired()
			.HasConversion(
				x => x.ToString(), 
				y => Enum.Parse<ScheduleWeeksType>(y) 
			);

		builder.HasOne(x => x.Institution)
			.WithMany(x => x.Schedules)
			.HasForeignKey(x => x.InstitutionId);

		builder.HasMany(x => x.Members)
			.WithOne(x => x.Schedule)
			.HasForeignKey(x => x.ScheduleId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.HasMany(x => x.Lessons)
			.WithOne(x => x.Schedule)
			.HasForeignKey(x => x.ScheduleId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.Navigation(p => p.Members).UsePropertyAccessMode(PropertyAccessMode.Field);
		builder.Navigation(p => p.Lessons).UsePropertyAccessMode(PropertyAccessMode.Field);
	}
}
