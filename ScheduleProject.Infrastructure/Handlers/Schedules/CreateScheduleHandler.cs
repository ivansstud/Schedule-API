using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ScheduleProject.Application.Requests.Schedules;
using ScheduleProject.Core.Entities;
using ScheduleProject.Core.Entities.ValueObjects;
using ScheduleProject.Infrastructure.Auth.Enums;
using ScheduleProject.Infrastructure.Auth.Extensions;
using ScheduleProject.Infrastructure.DAL.Services;
using ScheduleProject.Infrastructure.Handlers.Lessons;
using System.Security.Claims;

namespace ScheduleProject.Infrastructure.Handlers.Schedules;

public class CreateScheduleHandler : IRequestHandler<CreateScheduleCommand, Result>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ILogger<CreateScheduleHandler> _logger;
	private readonly ClaimsPrincipal _user;

	public CreateScheduleHandler(IUnitOfWork unitOfWork, ILogger<CreateScheduleHandler> logger, ClaimsPrincipal user)
	{
		_unitOfWork = unitOfWork;
		_logger = logger;
		_user = user;
	}

	public async Task<Result> Handle(CreateScheduleCommand request, CancellationToken cancellationToken)
	{
		try
		{
			var userId = _user.GetId();
			var scheduleType = ScheduleType.FromValue(request.Type);
			var weekType = ScheduleWeeksType.FromValue(request.WeeksType);

			if (scheduleType is null || weekType is null)
			{
				_logger.LogError("Неудачная попытка создания занятия для пользователя userId: {userId}, scheduleType: {scheduleType}, weekType: {weekType}", userId, request.Type, request.WeeksType);
				return Result.Failure("Упс! Не удалось создать расписание.");
			}

			var newScheduleResult = Schedule.Create(
				name: request.Name,
				description: request.Description,
				type: scheduleType,
				weeksType: weekType,
				institutionId: request.InstitutionId);

			if (!newScheduleResult.TryGetValue(out Schedule? schedule))
			{
				return Result.Failure(newScheduleResult.Error);
			}

			var user = await _unitOfWork.Db
				.Set<AppUser>()
				.Where(x => x.Id == userId && x.IsDeleted == false)
				.FirstOrDefaultAsync(cancellationToken);

			if (user == null)
			{
				_logger.LogError("Не удалось найти пользователя в БД по Id в токене. UserId: {Id}:", userId);
				return Result.Failure("Упс! Непредвиденная ошибка. Необходимо повторить вход.");
			}

			await _unitOfWork.BeginTransactionAsync(cancellationToken);

			if (schedule.InstitutionId != null)
			{
				var isInstitusionFound = await _unitOfWork.Db
					.Set<Institution>()
					.Where(x => x.Id == schedule.InstitutionId && x.IsDeleted == false)
					.AnyAsync(cancellationToken);

				if (isInstitusionFound == false)
				{
					return Result.Failure("Упс! В данный момент создать расписание для данного учреждения невозможно.");
				}
			}

			await _unitOfWork.Db.AddAsync(schedule, cancellationToken);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			var userMember = ScheduleMember.Create(schedule.Id, userId, ScheduleRole.Creator);
			user.AddMemberships(userMember);

			await _unitOfWork.SaveChangesAsync(cancellationToken);
			await _unitOfWork.CommitAsync(cancellationToken);

			return Result.Success();
		}
		catch (Exception ex)
		{
			_logger.LogError("{Exception}:", ex.Message + ex.InnerException?.Message);

			await _unitOfWork.RollbackAsync();

			return Result.Failure("Упс! Не удалось добавить расписание.");
		}
	}
}
