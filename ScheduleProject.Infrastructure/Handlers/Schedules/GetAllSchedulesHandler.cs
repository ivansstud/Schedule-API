using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ScheduleProject.Application.DTOs.Lesson;
using ScheduleProject.Application.DTOs.Schedules;
using ScheduleProject.Application.DTOs.ValueObjects;
using ScheduleProject.Application.Requests.Schedules;
using ScheduleProject.Core.Entities;
using ScheduleProject.Core.Entities.Enums;
using ScheduleProject.Core.Entities.ValueObjects;
using ScheduleProject.Infrastructure.Auth.Extensions;
using ScheduleProject.Infrastructure.DAL.Services;
using ScheduleProject.Infrastructure.Extensions.Mapping;
using ScheduleProject.Infrastructure.Handlers.Lessons;
using System.Security.Claims;

namespace ScheduleProject.Infrastructure.Handlers.Schedules;

public class GetAllSchedulesHandler : IRequestHandler<GetAllSchedulesRequest, Result<ScheduleDto[]>>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ILogger<GetAllLesonsHandler> _logger;
	private readonly ClaimsPrincipal _user;

	public GetAllSchedulesHandler(IUnitOfWork unitOfWork, ILogger<GetAllLesonsHandler> logger, ClaimsPrincipal user)
	{
		_unitOfWork = unitOfWork;
		_logger = logger;
		_user = user;
	}

	public async Task<Result<ScheduleDto[]>> Handle(GetAllSchedulesRequest request, CancellationToken cancellationToken)
	{
		try
		{
			var schedules = await _unitOfWork.Db
				.Set<Schedule>()
				.AsNoTracking()
				.Include(x => x.Members)
				.Include(x => x.Lessons)
				.Where(x => x.IsDeleted == false)
				.Select(x => new ScheduleDto
				{
					Id = x.Id,
					Name = x.Name,
					Description = x.Description,
					Type = x.Type.MapToDto(),
					WeeksType = x.WeeksType.MapToDto(),
					InstitutionId = x.InstitutionId,
					OwnerId = x.Members.First(x => x.Role == ScheduleRole.Creator).UserId,
					Members = x.Members.Select(m => m.MapToDto()).ToArray(),
					Lessons = x.Lessons.Select(l => l.MapToFullDto()).ToArray()
				})
				.ToArrayAsync(cancellationToken);

			return schedules;
		}
		catch (Exception ex)
		{
			_logger.LogError("{Exception}", ex.Message + ex.InnerException?.Message);

			return Result.Failure<ScheduleDto[]>("Упс! Не удалось загрузить расписания.");
		}
	}
}
