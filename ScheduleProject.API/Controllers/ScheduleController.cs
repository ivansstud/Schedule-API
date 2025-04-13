using Microsoft.AspNetCore.Mvc;
using ScheduleProject.Infrastracture.EF;

namespace ScheduleProject.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
	public class ScheduleController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

		public ScheduleController(AppDbContext dbContext)
		{
			_dbContext = dbContext;
		}



		
	}
}
