using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScheduleProject.Core.Entities;

namespace ScheduleProject.Infrastracture.EF.Configurations;

class TelegramUserConfiguration : IEntityTypeConfiguration<TelegramUser>
{
	public void Configure(EntityTypeBuilder<TelegramUser> builder)
	{
		builder.HasKey(x => x.Id);
		builder.Property(x => x.Id).ValueGeneratedNever();

		builder.Property(x => x.FirstName).HasMaxLength(TelegramUser.MaxFirstNameLength);
		builder.Property(x => x.LastName).HasMaxLength(TelegramUser.MaxLastNameLength);
		builder.Property(x => x.UserName).IsRequired().HasMaxLength(TelegramUser.MaxUserNameLength);

		builder.HasMany(x => x.ScheduleMemberships)
			.WithOne(x => x.TelegramUser)
			.HasForeignKey(x => x.TelegramUserId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.Navigation(x => x.ScheduleMemberships).UsePropertyAccessMode(PropertyAccessMode.Field);
	}
}
