using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScheduleProject.Core.Entities;
using ScheduleProject.Core.Entities.Enums;
using ScheduleProject.Core.Entities.ValueObjects;

namespace ScheduleProject.Infrastructure.DAL.EF.Configurations;

class ScheduleMemberConfiguration : IEntityTypeConfiguration<ScheduleMember>
{
	public void Configure(EntityTypeBuilder<ScheduleMember> builder)
	{
		builder.HasKey(x => x.Id);
		builder.Property(x => x.Id).ValueGeneratedOnAdd();

		builder.Property(x => x.Role)
			.IsRequired()
			.HasMaxLength(ScheduleRole.MaxLength)
			.HasConversion(
				x => x.Name,
				y => ScheduleRole.FromName(y)!
			);

		builder.HasOne(x => x.Schedule)
			.WithMany(x => x.Members)
			.HasForeignKey(x => x.ScheduleId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.HasOne(x => x.User)
			.WithMany(x => x.ScheduleMemberships)
			.HasForeignKey(x => x.UserId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}
