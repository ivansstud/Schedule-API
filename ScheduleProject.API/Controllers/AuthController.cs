using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScheduleProject.Core.Entities;
using ScheduleProject.Infrastracture.EF;

namespace ScheduleProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

		public AuthController(AppDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public record TelegramDataRequest(long Id, string? FirstName, string? LastName, string UserName, string? PhotoUrl);

        [HttpPost("[action]")]
        public async Task<IActionResult> AuthTelegramData(TelegramDataRequest request, CancellationToken cancellationToken = default)
        {
            var user = _dbContext.TelegramUsers.FirstOrDefault(x => x.Id == request.Id);

            if (user == null)
            {
                var newUserResult = TelegramUser.Create(request.Id,
                    request.FirstName,
                    request.LastName,
                    request.UserName,
                    request.PhotoUrl
                );

                if (newUserResult.IsFailure)
                {
                    return BadRequest(newUserResult.Error);
                }

                await _dbContext.AddAsync(newUserResult.Value, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }

            return Ok();
        }
    }
}
