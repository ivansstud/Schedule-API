using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScheduleProject.Core.Entities;

namespace ScheduleProject.Infrastructure.DAL.EF.Configurations;

class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
	public void Configure(EntityTypeBuilder<AppUser> builder)
	{
		builder.HasKey(x => x.Id);
		builder.Property(x => x.Id).ValueGeneratedOnAdd();

		builder.Property(x => x.FirstName).IsRequired().HasMaxLength(AppUser.MaxFirstNameLength);
		builder.Property(x => x.LastName).IsRequired().HasMaxLength(AppUser.MaxLastNameLength);
		builder.Property(x => x.Login).IsRequired().HasMaxLength(AppUser.MaxLoginLength);
		builder.Property(x => x.HashedPassword).IsRequired().HasMaxLength(AppUser.MaxHashedPasswordLength);
		builder.Property(x => x.AuthTokenId).IsRequired(false);

		builder.HasOne(x => x.AuthToken)
			.WithOne(x => x.Owner)
			.HasForeignKey<AuthToken>(x => x.OwnerId);

		builder.HasIndex(x => x.Login).IsUnique();

		builder.HasMany(x => x.ScheduleMemberships)
			.WithOne(x => x.User)
			.HasForeignKey(x => x.UserId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.Navigation(x => x.ScheduleMemberships).UsePropertyAccessMode(PropertyAccessMode.Field);
		builder.Navigation(x => x.Roles).UsePropertyAccessMode(PropertyAccessMode.Field);
	}
}
