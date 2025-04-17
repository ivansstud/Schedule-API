using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScheduleProject.API.Dtos.Responce.UserRole;
using ScheduleProject.API.Extensions.Mapping;
using ScheduleProject.Core.Abstractions.Services;
using ScheduleProject.Core.Entities.Enums;

namespace ScheduleProject.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = AppRoles.Administrator)]
public class UserRolesController : ControllerBase
{

	private readonly IUserRolesService _rolesService;

	public UserRolesController(IUserRolesService rolesService)
	{
		_rolesService = rolesService;
	}

	[HttpGet]
	public async Task<List<UserRoleResponce>> GetAll(CancellationToken cancellationToken)
	{
		var roles = await _rolesService.GetAll(false, cancellationToken);

		return roles.Select(role => role.MapToResponce()).ToList();
	}

	[HttpPost("[action]")]
	public async Task<IActionResult> Give(GiveRoleRequest request, CancellationToken cancellationToken)
	{
		var result = await _rolesService.GiveToUser(request.UserId, request.RoleId, cancellationToken);

		if (result.IsFailure)
		{
			return BadRequest(result.Error);
		}

		return Ok();
	}

	[HttpPost("[action]")]
	public async Task<IActionResult> Take(RemoveRoleRequest request, CancellationToken cancellationToken)
	{
		var result = await _rolesService.TakeFromUser(request.UserId, request.RoleId, cancellationToken);

		if (result.IsFailure)
		{
			return BadRequest(result.Error);
		}

		return Ok();
	}

	public sealed record GiveRoleRequest(long UserId, long RoleId);
	public sealed record RemoveRoleRequest(long UserId, long RoleId);
}
