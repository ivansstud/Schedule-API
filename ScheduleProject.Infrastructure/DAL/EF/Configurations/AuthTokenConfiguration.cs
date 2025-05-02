using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScheduleProject.Core.Entities;

namespace ScheduleProject.Infrastructure.DAL.EF.Configurations;

internal class AuthTokenConfiguration : IEntityTypeConfiguration<AuthToken>
{
	public void Configure(EntityTypeBuilder<AuthToken> builder)
	{
		builder.HasOne(x => x.Owner)
			.WithOne(x => x.AuthToken)
			.HasForeignKey<AppUser>(x => x.AuthTokenId);
	}
}
