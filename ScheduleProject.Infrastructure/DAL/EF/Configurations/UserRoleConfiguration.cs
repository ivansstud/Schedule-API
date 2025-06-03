using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScheduleProject.Core.Entities;
using ScheduleProject.Core.Entities.Enums;

namespace ScheduleProject.Infrastructure.DAL.EF.Configurations;

class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
	public void Configure(EntityTypeBuilder<UserRole> builder)
	{
		builder.HasKey(x => x.Id);
		builder.Property(x => x.Id).ValueGeneratedNever();

		builder.Property(x => x.Name).IsRequired().HasMaxLength(UserRole.MaxNameLength);
		builder.HasIndex(x => x.Name).IsUnique();

		builder.HasMany(ur => ur.Users)
			.WithMany(u => u.Roles)
			.UsingEntity("Users_Roles");

		builder.Navigation(x => x.Users).UsePropertyAccessMode(PropertyAccessMode.Field);

		AddRoles(builder);
	}

	private static void AddRoles(EntityTypeBuilder<UserRole> builder)
	{
		var appRoles = AppRoles.GetRoles();
		var date = new DateTime(2025, 5, 1).ToUniversalTime();

		for (int i = 0; i < appRoles.Count; i++)
		{
			builder.HasData(UserRole.Create(
					id: i + 1,
					name: appRoles[i],
					createDate: date));
		}
	}
}
