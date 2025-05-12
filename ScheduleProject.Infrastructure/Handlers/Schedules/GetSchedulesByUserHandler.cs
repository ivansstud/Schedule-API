using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ScheduleProject.Application.DTOs.Schedules;
using ScheduleProject.Application.Requests.Schedules;
using ScheduleProject.Core.Entities;
using ScheduleProject.Infrastructure.Auth.Extensions;
using ScheduleProject.Infrastructure.DAL.Services;
using ScheduleProject.Infrastructure.Extensions.Mapping;
using System.Security.Claims;

namespace ScheduleProject.Infrastructure.Handlers.Schedules;

public class GetSchedulesByUserHandler : IRequestHandler<GetSchedulesByUserRequest, Result<ScheduleByUserDto[]>>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ILogger<GetSchedulesByUserHandler> _logger;
	private readonly ClaimsPrincipal _user;

	public GetSchedulesByUserHandler(IUnitOfWork unitOfWork, ILogger<GetSchedulesByUserHandler> logger, ClaimsPrincipal user)
	{
		_unitOfWork = unitOfWork;
		_logger = logger;
		_user = user;
	}

	public async Task<Result<ScheduleByUserDto[]>> Handle(GetSchedulesByUserRequest request, CancellationToken cancellationToken)
	{
		try
		{
			var currentUserId = _user.GetId();
			var requestUserId = request.UserId;

			if (!_user.IsAdmin() && currentUserId != requestUserId)
			{
				//TODO: Лог этого момента
				return Result.Failure<ScheduleByUserDto[]>("Вы не можете смотреть расписание этого пользователя.");
			}

			var schedules = await _unitOfWork.Db
				.Set<ScheduleMember>()
				.AsNoTracking()
				.Include(x => x.Schedule)
				.Where(x => x.UserId == requestUserId && x.Schedule.IsDeleted == false)
				.Select(x => x.MapToScheduleByUserDto())
				.ToArrayAsync(cancellationToken);

			return schedules;
		}
		catch (Exception ex)
		{
			_logger.LogError("{Exception}", ex.Message + ex.InnerException?.Message);

			return Result.Failure<ScheduleByUserDto[]>("Упс! Не удалось загрузить расписания.");
		}
	}
}

