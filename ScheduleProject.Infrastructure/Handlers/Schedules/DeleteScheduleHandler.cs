using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.Logging;
using ScheduleProject.Application.Requests.Schedules;
using ScheduleProject.Core.Entities.ValueObjects;
using ScheduleProject.Core.Entities;
using ScheduleProject.Infrastructure.Auth.Extensions;
using ScheduleProject.Infrastructure.DAL.Services;
using ScheduleProject.Infrastructure.Handlers.Lessons;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using ScheduleProject.Core.Entities.Enums;

namespace ScheduleProject.Infrastructure.Handlers.Schedules;

public class DeleteScheduleHandler : IRequestHandler<DeleteScheduleCommand, Result>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ILogger<DeleteScheduleHandler> _logger;
	private readonly ClaimsPrincipal _user;

	public DeleteScheduleHandler(IUnitOfWork unitOfWork, ILogger<DeleteScheduleHandler> logger, ClaimsPrincipal user)
	{
		_unitOfWork = unitOfWork;
		_logger = logger;
		_user = user;
	}

	public async Task<Result> Handle(DeleteScheduleCommand request, CancellationToken cancellationToken)
	{
		try
		{
			var userId = _user.GetId();
			var isUserAdmin = _user.IsAdmin();
			var scheduleId = request.Id;

			var scheduleForDelete = await _unitOfWork.Db
				.Set<Schedule>()
				.Include(x => x.Members)
				.Where(x => x.Id == scheduleId)
				.FirstOrDefaultAsync(cancellationToken);

			if (scheduleForDelete == null)
			{
				_logger.LogError("Не найдено расписания для удаления. Пользователь {userId} хотел удалить {scheduleId}:", userId, scheduleId);
				return Result.Failure("Данного расписания не найдено.");
			}

			if (scheduleForDelete.IsDeleted)
			{
				return Result.Success();
			}

			if (isUserAdmin == false)
			{
				var isUserCreator = scheduleForDelete.Members
					.Any(x => x.UserId == userId && x.Role == ScheduleRole.Creator);

				if (isUserCreator == false)
				{
					return Result.Failure("Вы не имеете прав для удаления данного расписания.");
				}
			}

			await _unitOfWork.BeginTransactionAsync(cancellationToken);
			
			scheduleForDelete.SetDeleted(true);

			await _unitOfWork.SaveChangesAsync(cancellationToken);
			await _unitOfWork.CommitAsync(cancellationToken);

			return Result.Success();
		}
		catch (Exception ex)
		{
			_logger.LogError("{Exception}:", ex.Message + ex.InnerException?.Message);

			await _unitOfWork.RollbackAsync();

			return Result.Failure("Упс! Не удалось удалить это расписание.");
		}
	}
}
