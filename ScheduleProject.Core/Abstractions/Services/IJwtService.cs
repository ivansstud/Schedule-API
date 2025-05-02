using CSharpFunctionalExtensions;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace ScheduleProject.Core.Abstractions.Services;

public interface IJwtService
{
	string CreateAccessToken(long userId, string login, string name, List<string> roles);
	string CreateRefreshToken();
	Result<ClaimsPrincipal> GetPrincipalFromToken(string token, TokenValidationParameters parameters);
}
