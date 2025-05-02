using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScheduleProject.Core.Entities;

namespace ScheduleProject.Infrastructure.DAL.EF.Configurations;

class InstitutionConfiguration : IEntityTypeConfiguration<Institution>
{
	public void Configure(EntityTypeBuilder<Institution> builder)
	{
		builder.HasKey(x => x.Id);
		builder.Property(x => x.Id).ValueGeneratedOnAdd();

		builder.Property(x => x.Name)
			.IsRequired()
			.HasMaxLength(Institution.MaxNameLength);

		builder.Property(x => x.ShortName)
			.IsRequired()
			.HasMaxLength(Institution.MaxShortNameLength);

		builder.HasIndex(x => x.Name).IsUnique();
		builder.HasIndex(x => x.ShortName).IsUnique();

		builder.Property(x => x.Description)
			.IsRequired(false)
			.HasMaxLength(Institution.MaxDescriptionLength);

		builder.HasMany(x => x.Schedules)
			.WithOne(x => x.Institution)
			.HasForeignKey(x => x.InstitutionId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.Navigation(x => x.Schedules).UsePropertyAccessMode(PropertyAccessMode.Field);
	}
}
