using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ScheduleProject.Core.Abstractions.Services;
using ScheduleProject.Core.Entities.Enums;
using ScheduleProject.Core.Entities;
using ScheduleProject.Infrastracture.Auth.Options;
using ScheduleProject.Infrastracture.EF;
using ScheduleProject.Core.Dtos.Auth;
using Microsoft.EntityFrameworkCore;
using ScheduleProject.API.Dtos.Responce.Auth;

namespace ScheduleProject.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
	private readonly JwtOptions _jwtOptions;
	private readonly AppDbContext _dbContext;
	private readonly IAuthService _authService;

	public AuthController(IAuthService authService, IOptions<JwtOptions> jwtOptions, AppDbContext db)
	{
		_dbContext = db;
		_authService = authService;
		_jwtOptions = jwtOptions.Value;
	}

	[HttpPost("[action]")]
	public async Task<ActionResult<UserResponceDto>> Login(UserLoginDto loginDto, CancellationToken cancellationToken = default)
	{
		var loginResult = await _authService.LoginAsync(loginDto, cancellationToken);

		if (loginResult.IsFailure)
		{
			return BadRequest(loginResult.Error);
		}

		var user = loginResult.Value;

		AddAuthTokenToCookie(user.AuthToken!);

		return new UserResponceDto 
		{
			Login = user.Login,
			FirstName = user.FirstName,
			LastName = user.LastName,
			Roles = user.Roles.Select(x => x.Name).ToArray(),
		};
	}

	[HttpPost("[action]")]
	public async Task<IActionResult> Register(UserRegisterDto registerDto, CancellationToken cancellationToken = default)
	{
		var registerResult = await _authService.RegisterAsync(registerDto, cancellationToken);

		if (registerResult.IsFailure)
		{
			return BadRequest(registerResult.Error);
		}

		return Ok();
	}

	[HttpGet("[action]")]
	public ActionResult<UserResponceDto[]> Users()
	{
		var users = _dbContext.Users.Include(x => x.Roles).AsNoTracking().ToList();
		var responce = users.Select(user => new UserResponceDto
		{
			Login = user.Login,
			FirstName = user.FirstName,
			LastName = user.LastName,
			Roles = user.Roles.Select(x => x.Name).ToArray(),
		});

		return Ok(responce);
	}

	[HttpPost("[action]")]
	public async Task<IActionResult> Refresh(RefreshDto refreshDto)
	{
		var refreshToken = HttpContext.Request.Cookies[_jwtOptions.RefreshTokenCookieKey];

		if (refreshToken is null)
		{
			return BadRequest("Ошибка авторизации. Повторите вход");
		}

		var refreshTokensDto = new RefreshTokensDto { Login = refreshDto.Login, RefreshToken = refreshToken };
		var tokenResult = await _authService.RefreshTokens(refreshTokensDto);

		if (tokenResult.IsFailure)
		{
			return BadRequest(tokenResult.Error);
		}

		AddAuthTokenToCookie(tokenResult.Value);

		return Ok();
	}

	[Authorize(Roles = $"{AppRoles.DomainUser}")]
	[HttpPost("[action]")]
	public ActionResult<string> OnlyDomainAndAdmin()
	{
		return "success";
	}

	[Authorize(Roles = AppRoles.Administrator)]
	[HttpPost("[action]")]
	public ActionResult<string> OnlyAdmin()
	{
		return "success";
	}

	public sealed record RefreshDto(string Login);

	private void AddAuthTokenToCookie(AuthToken token)
	{
		HttpContext.Response.Cookies.Append(_jwtOptions.AccessTokenCookieKey, token.AccessToken);
		HttpContext.Response.Cookies.Append(_jwtOptions.RefreshTokenCookieKey, token.RefreshToken);
	}
}
