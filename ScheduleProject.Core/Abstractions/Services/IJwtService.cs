using CSharpFunctionalExtensions;
using Microsoft.IdentityModel.Tokens;
using ScheduleProject.Core.Dtos.Auth;
using System.Security.Claims;

namespace ScheduleProject.Core.Abstractions.Services;

public interface IJwtService
{
	string CreateAccessToken(CreateAccessTokenDto createTokenDto);
	string CreateRefreshToken();
	Result<ClaimsPrincipal> GetPrincipalFromToken(string token, TokenValidationParameters parameters);
}
