using CSharpFunctionalExtensions;
using ScheduleProject.Core.Dtos.Auth;
using ScheduleProject.Core.Entities;

namespace ScheduleProject.Core.Abstractions.Services;

public interface IAuthService
{
	Task<Result<AppUser>> LoginAsync(UserLoginDto loginDto, CancellationToken cancellationToken = default);

	Task<Result> RegisterAsync(UserRegisterDto registerDto, CancellationToken cancellationToken = default);

	Task<Result<AuthToken>> RefreshTokens(RefreshTokensDto refreshDto, CancellationToken cancellationToken = default);
}
