using Microsoft.AspNetCore.Mvc;
using ScheduleProject.Infrastracture.EF;

namespace ScheduleProject.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
	//[Authorize("TelegramUsers")]
	public class ScheduleController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

		public ScheduleController(AppDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		//[HttpGet("[action]")]
		//public async Task<ActionResult<UserMembershipsDto[]>> GetUserSchedules(long telegramUserId, CancellationToken cancellationToken = default)
		//{
		//	var schedules = await _dbContext.ScheduleMembers
		//		.AsNoTracking()
		//		.Include(sm => sm.Schedule)
		//		.Where(sm => sm.TelegramUserId == telegramUserId)
		//		.Select(sm => new UserMembershipsDto
		//		{
		//			Id = sm.Schedule.Id,
		//			Name = sm.Schedule.Name,
		//			Type = sm.Schedule.Type,
		//			Role = sm.Role
		//		})
		//		.ToListAsync(cancellationToken);

		//	return Ok(schedules);
		//}

		
	}
}
