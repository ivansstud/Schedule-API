using ScheduleProject.Infrastructure.Auth.Enums;
using System.Security.Claims;

namespace ScheduleProject.Infrastructure.Auth.Extensions;

public static class ClaimsPrincipalExtesions
{
	public static long GetId(this ClaimsPrincipal principal)
	{
		return long.Parse(principal.FindFirstValue(CustomClaimTypes.Id)!);
	}

	public static string GetFirstName(this ClaimsPrincipal principal)
	{
		return principal.FindFirstValue(CustomClaimTypes.FirstName)!;
	}

	public static string GetLogin(this ClaimsPrincipal principal)
	{
		return principal.FindFirstValue(CustomClaimTypes.Login)!;
	}

	public static string[] GetRoles(this ClaimsPrincipal principal)
	{
		return principal.FindAll(CustomClaimTypes.Role).Select(x => x.Value).ToArray();
	}
}
