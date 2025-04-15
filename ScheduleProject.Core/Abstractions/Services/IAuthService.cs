using CSharpFunctionalExtensions;
using ScheduleProject.Core.Dtos.Auth;
using ScheduleProject.Core.Entities;

namespace ScheduleProject.Core.Abstractions.Services;

public interface IAuthService
{
	Task<Result> RegisterAsync(UserRegisterDto registerDto, CancellationToken cancellationToken = default);

	Task<Result<AppUser>> LoginAsync(UserLoginDto loginDto, CancellationToken cancellationToken = default);

	Task<Result<AuthToken>> RefreshTokensAsync(RefreshTokensDto refreshDto, CancellationToken cancellationToken = default);

	string? GetLoginFromExpiredToken(string jwtToken);
}
