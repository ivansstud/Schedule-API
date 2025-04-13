using Microsoft.AspNetCore.Identity;
using ScheduleProject.Core.Abstractions.Services;

namespace ScheduleProject.Core.Services;

public class PasswordService : IPasswordService
{
	public string Hash(string password)
	{
		return new PasswordHasher<object>().HashPassword(null!, password);
	}

	public bool Verify(string hashedPassword, string providedPassword)
	{
		var result = new PasswordHasher<object>().VerifyHashedPassword(null!, hashedPassword, providedPassword);

		if (result == PasswordVerificationResult.Failed)
		{
			return false;
		}

		return true;
	}
}
