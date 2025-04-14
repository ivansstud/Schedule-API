using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScheduleProject.Core.Entities.Enums;
using ScheduleProject.Infrastracture.EF;

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




}
