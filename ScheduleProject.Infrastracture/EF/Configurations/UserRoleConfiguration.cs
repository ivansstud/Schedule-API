using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScheduleProject.Core.Entities;
using ScheduleProject.Core.Entities.Enums;

namespace ScheduleProject.Infrastracture.EF.Configurations;

class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
	public void Configure(EntityTypeBuilder<UserRole> builder)
	{
		builder.HasKey(x => x.Id);

		builder.Property(x => x.Name).IsRequired().HasMaxLength(UserRole.MaxNameLength);
		builder.HasIndex(x => x.Name).IsUnique();

		builder.HasMany(ur => ur.Users)
			.WithMany(u => u.Roles)
			.UsingEntity("Users_Roles");

		builder.Navigation(x => x.Users).UsePropertyAccessMode(PropertyAccessMode.Field);

		AddRolesData(builder);
	}

	private static void AddRolesData(EntityTypeBuilder<UserRole> builder)
	{
		var appRoles = AppRoles.GetRoles();

		Enumerable.Range(0, appRoles.Count)
			.ToList()
			.ForEach(index =>
			{
				builder.HasData(UserRole.Create(id: (index + 1), name: appRoles[index]));
			});
	}
}
