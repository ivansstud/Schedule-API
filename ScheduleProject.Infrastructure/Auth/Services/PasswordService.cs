using Microsoft.AspNetCore.Identity;
using ScheduleProject.Core.Abstractions.Services;

namespace ScheduleProject.Infrastructure.Auth.Services;

public class PasswordService : IPasswordService
{
	public string Hash(string password)
	{
		var passwordHash = new PasswordHasher<object>().HashPassword(null!, password);
		return passwordHash;
	}

	public bool Verify(string passwordHash, string password)
	{
		var result = new PasswordHasher<object>().VerifyHashedPassword(null!, passwordHash, password);

		if (result == PasswordVerificationResult.Failed)
		{
			return false;
		}

		return true;
	}
}
