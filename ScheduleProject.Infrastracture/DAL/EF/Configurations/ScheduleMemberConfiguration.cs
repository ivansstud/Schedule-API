using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScheduleProject.Core.Entities;
using ScheduleProject.Core.Entities.Enums;

namespace ScheduleProject.Infrastracture.DAL.EF.Configurations;

class ScheduleMemberConfiguration : IEntityTypeConfiguration<ScheduleMember>
{
	public void Configure(EntityTypeBuilder<ScheduleMember> builder)
	{
		builder.HasKey(x => x.Id);
		builder.Property(x => x.Id).ValueGeneratedOnAdd();

		builder.Property(x => x.Role)
			.IsRequired()
			.HasConversion(
				x => x.ToString(), // Преобразование Enum -> String
				y => Enum.Parse<ScheduleRole>(y) // Преобразование String -> Enum
			);

		builder.HasOne(x => x.Schedule)
			.WithMany(x => x.Members)
			.HasForeignKey(x => x.ScheduleId);

		builder.HasOne(x => x.User)
			.WithMany(x => x.ScheduleMemberships)
			.HasForeignKey(x => x.UserId);
	}
}
