using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ScheduleProject.Core.Abstractions.Services;
using ScheduleProject.Core.Entities.Enums;
using ScheduleProject.Core.Entities;
using ScheduleProject.Infrastracture.Auth.Options;
using ScheduleProject.Core.Dtos.Auth;
using Microsoft.EntityFrameworkCore;
using ScheduleProject.API.Dtos.Responce.Auth;
using System.Data;
using System.Security.Claims;
using ScheduleProject.Infrastracture.DAL.EF;

namespace ScheduleProject.API.Controllers;

[ApiController]
[Route("api/[controller]")]
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
	public async Task<ActionResult<UserResponce>> Login(UserLoginDto loginDto, CancellationToken cancellationToken = default)
	{
		var loginResult = await _authService.LoginAsync(loginDto, cancellationToken);

		if (loginResult.IsFailure)
		{
			return BadRequest(loginResult.Error);
		}

		var user = loginResult.Value;

		AddAuthTokenToCookie(user.AuthToken!);

		return new UserResponce 
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

	[HttpPost("[action]")]
	public  IActionResult Logout()
	{
		Response.Cookies.Delete(_jwtOptions.AccessTokenCookieKey);
		Response.Cookies.Delete(_jwtOptions.RefreshTokenCookieKey);

		return Ok();
	}

	[HttpGet("[action]")]
	public ActionResult<UserResponce[]> Users()
	{
		var users = _dbContext.Users.Include(x => x.Roles).AsNoTracking().ToList();
		var responce = users.Select(user => new UserResponce
		{
			Login = user.Login,
			FirstName = user.FirstName,
			LastName = user.LastName,
			Roles = user.Roles.Select(x => x.Name).ToArray(),
		});

		return Ok(responce);
	}

	[HttpPost("[action]")]
	public async Task<IActionResult> Refresh()
	{
		var refreshToken = Request.Cookies[_jwtOptions.RefreshTokenCookieKey];
		var accessToken = Request.Cookies[_jwtOptions.AccessTokenCookieKey];

		if (refreshToken is null || accessToken is null)
		{
			return BadRequest("Ошибка авторизации. Повторите вход");
		}

		var login = _authService.GetLoginFromExpiredToken(accessToken);

		if (login is null)
		{
			return BadRequest("Ошибка авторизации. Повторите вход");
		}

		var tokenResult = await _authService.RefreshTokensAsync(new (Login: login, RefreshToken: refreshToken));

		if (!tokenResult.TryGetValue(out var token))
		{
			return BadRequest(tokenResult.Error);
		}

		AddAuthTokenToCookie(token);

		return Ok();
	}

	[Authorize]
	[HttpPost("[action]")]
	public IActionResult GetCurrentUser()
	{
		return Ok(new
		{
			Id = User.FindFirstValue(ClaimTypes.NameIdentifier),
			Name = User.Identity?.Name,
			Login = User.FindFirstValue(CustomClaimTypes.Login),
			Roles = User.FindAll(ClaimTypes.Role).Select(x => x.Value),
		});
	}

	private void AddAuthTokenToCookie(AuthToken token)
	{
		Response.Cookies.Append(_jwtOptions.AccessTokenCookieKey, token.AccessToken);
		Response.Cookies.Append(_jwtOptions.RefreshTokenCookieKey, token.RefreshToken);
	}
}
