using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScheduleProject.Core.Entities.Enums;
using ScheduleProject.Infrastracture.DAL.EF;

namespace ScheduleProject.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = AppRoles.Administrator)]
public class UserRolesController : ControllerBase
{
	private readonly AppDbContext _dbContext;

	public UserRolesController(AppDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	[HttpPost("[action]")]
	public async Task<IActionResult> Give(GiveRoleRequest request, CancellationToken cancellationToken)
	{
		var user = await _dbContext.Users.FindAsync(request.UserId, cancellationToken);

		if (user is null)
		{
			return BadRequest("Пользователя с таким Id не существует");
		}

		var role = await _dbContext.UserRoles.FindAsync(request.RoleId, cancellationToken);

		if (role is null)
		{
			return BadRequest("Роли с таким Id не существует");
		}

		user.AddRole(role);
		await _dbContext.SaveChangesAsync(cancellationToken);

		return Ok();
	}

	public sealed record GiveRoleRequest(long UserId, long RoleId);
}
