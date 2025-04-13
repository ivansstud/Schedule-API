using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScheduleProject.Infrastracture.EF;

namespace ScheduleProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

		public ScheduleController(AppDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		[HttpPost("[action]")]
		public async Task<IActionResult> Lookup([FromBody] long telegramUserId, CancellationToken cancellationToken = default)
		{
			var schedules = await _dbContext.ScheduleMembers
				.AsNoTracking()
				.Include(sm => sm.Schedule)
				.Where(sm => sm.TelegramUserId == telegramUserId)
				.Select(sm => new
				{
					sm.Role,
					sm.Schedule.Type,
					sm.Schedule.Name,
					Id = sm.ScheduleId,
				})
				.ToListAsync();

			return Ok(schedules);
		}
	}
}
