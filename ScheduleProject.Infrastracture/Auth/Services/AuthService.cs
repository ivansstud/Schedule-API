using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ScheduleProject.Core.Abstractions.Services;
using ScheduleProject.Core.Dtos.Auth;
using ScheduleProject.Core.Entities;
using ScheduleProject.Core.Entities.Enums;
using ScheduleProject.Infrastracture.Auth.Options;
using ScheduleProject.Infrastracture.EF;

namespace ScheduleProject.Infrastracture.Auth.Services;

public class AuthService : IAuthService
{
	private readonly JwtOptions _jwtOptions;
	private readonly AppDbContext _dbContext;
	private readonly IJwtService _jwtService;
	private readonly IPasswordService _passwordService;

	public AuthService(AppDbContext dbContext, IJwtService jwtService, IPasswordService passwordService, IOptions<JwtOptions> jwtOptions)
	{
		_dbContext = dbContext;
		_jwtService = jwtService;
		_jwtOptions = jwtOptions.Value;
		_passwordService = passwordService;
	}

	public async Task<Result<AppUser>> LoginAsync(UserLoginDto loginDto, CancellationToken cancellationToken = default)
	{
		var foundUser = await _dbContext.Users
			.Include(x => x.AuthToken)
			.Include(x => x.Roles)
			.FirstOrDefaultAsync(x => x.Login == loginDto.Login, cancellationToken);

		if (foundUser is null)
		{
			return Result.Failure<AppUser>("Неверно введены логин или пароль");
		}

		var isVerifyPassword = _passwordService.Verify(foundUser.HashedPassword, loginDto.Password);

		if (isVerifyPassword == false)
		{
			return Result.Failure<AppUser>("Неверно введены логин или пароль");
		}

		if (foundUser.AuthToken is not null)
		{
			UpdateAuthToken(foundUser);
		}
		else
		{
			var roles = foundUser.Roles.Select(x => x.Name).ToList();
			var authToken = CreateAuthToken(foundUser.Id, foundUser.Login, roles);

			foundUser.SetAuthToken(authToken);
		}

		await _dbContext.SaveChangesAsync(cancellationToken);

		return foundUser;
	}

	public async Task<Result<AuthToken>> RefreshTokens(RefreshTokensDto refreshDto, CancellationToken cancellationToken = default)
	{
		var foundUser = await _dbContext.Users
			.Include(x => x.AuthToken)
			.FirstOrDefaultAsync(x => x.Login == refreshDto.Login, cancellationToken);

		var failureMessage = "Ошибка авторизации. Необходим повторный вход";

		if (foundUser is null || foundUser.AuthToken is null)
		{
			return Result.Failure<AuthToken>(failureMessage);
		}

		if (foundUser.AuthToken.RefreshToken != refreshDto.RefreshToken ||
			foundUser.AuthToken.RefreshTokenExpiryDate < DateTime.UtcNow)
		{
			return Result.Failure<AuthToken>(failureMessage);
		}

		UpdateAuthToken(foundUser);

		await _dbContext.SaveChangesAsync(cancellationToken);

		return foundUser.AuthToken;
	}

	public async Task<Result> RegisterAsync(UserRegisterDto registerDto, CancellationToken cancellationToken = default)
	{
		var userAlreadyExists = await _dbContext.Users
			.AsNoTracking()
			.AnyAsync(x => x.Login == registerDto.Login, cancellationToken);

		if (userAlreadyExists)
		{
			return Result.Failure("Пользоваетль с тами логиным уже существует");
		}

		var hashedPassword = _passwordService.Hash(registerDto.Password);
		var newUserResult = AppUser.Create(registerDto.Login, hashedPassword, registerDto.FirstName, registerDto.LastName);


		if (newUserResult.IsFailure)
		{
			return Result.Failure(newUserResult.Error);
		}

		var defaultRole = _dbContext.UserRoles.First(x => x.Name == AppRoles.DomainUser);

		var newUser = newUserResult.Value;
		newUser.AddRole(defaultRole);

		await _dbContext.AddAsync(newUser, cancellationToken);
		await _dbContext.SaveChangesAsync(cancellationToken);

		return Result.Success();
	}

	private void UpdateAuthToken(AppUser user)
	{
		var roles = user.Roles.Select(x => x.Name).ToList();
		var newAccessToken = _jwtService.CreateAccessToken(user.Id, user.FirstName, roles);
		var newRefreshToken = _jwtService.CreateRefreshToken();

		user.AuthToken!.Update(newAccessToken,
			newRefreshToken,
			_jwtOptions.AccessTokenExpiryMinutes,
			_jwtOptions.RefreshTokenExpiryDays);
	}

	private AuthToken CreateAuthToken(long userId, string userName, List<string> userRoles)
	{
		var accessToken = _jwtService.CreateAccessToken(userId, userName, userRoles);
		var refreshToken = _jwtService.CreateRefreshToken();
		var authToken = AuthToken.Create(
			ownerId: userId,
			accessToken: accessToken,
			refreshToken: refreshToken,
			accessTokenExpiryDate: DateTime.UtcNow.AddMinutes(_jwtOptions.AccessTokenExpiryMinutes),
			refreshTokenExpiryDate: DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenExpiryDays)
		);

		return authToken;
	}
}
