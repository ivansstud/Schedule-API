using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ScheduleProject.Application.Requests.Lessons;
using ScheduleProject.Core.Entities;
using ScheduleProject.Core.Entities.ValueObjects;
using ScheduleProject.Infrastructure.Auth.Extensions;
using ScheduleProject.Infrastructure.DAL.Services;
using System.Security.Claims;

namespace ScheduleProject.Infrastructure.Handlers.Lessons;

public class UpdateLessonHandler : IRequestHandler<UpdateLessonCommand, Result>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ILogger<UpdateLessonHandler> _logger;
	private readonly ClaimsPrincipal _user;

	public UpdateLessonHandler(IUnitOfWork unitOfWork, ILogger<UpdateLessonHandler> logger, ClaimsPrincipal user)
	{
		_unitOfWork = unitOfWork;
		_logger = logger;
		_user = user;
	}

	public async Task<Result> Handle(UpdateLessonCommand request, CancellationToken cancellationToken)
	{
		try
		{
			var lesson = await _unitOfWork.Db
				.Set<Lesson>()
				.Include(x => x.Schedule)
				.ThenInclude(x => x.Members)
				.Where(x => x.IsDeleted == false)
				.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

			if (lesson is null)
			{
				return Result.Failure("Не удалось найти занятие!");
			}

			if (lesson.Schedule.GetCreatorId() != _user.GetId() && !_user.IsAdmin())
			{
				return Result.Failure("У вас нет прав для изменения этого занятия!");
			}

			if (request.Name is not null && lesson.SetName(request.Name).TryGetError(out var nameError))
			{
				return Result.Failure(nameError);
			}
			if (request.Description is not null && lesson.SetDescription(request.Description).TryGetError(out var descriptionError))
			{
				return Result.Failure(descriptionError);
			}
			if (request.Audience is not null && lesson.SetAudience(request.Audience).TryGetError(out var audienceError))
			{
				return Result.Failure(audienceError);
			}
			if (request.TeacherName is not null && lesson.SetTeacherName(request.TeacherName).TryGetError(out var teacherNameError))
			{
				return Result.Failure(teacherNameError);
			}
			if (lesson.SetTime(request.StartTime, request.EndTime).TryGetError(out var timeError))
			{
				return Result.Failure(timeError);
			}

			var lessonTypeResult = LessonType.Create(request.LessonType);

			if (lessonTypeResult.IsFailure)
			{
				return Result.Failure(lessonTypeResult.Error);
			}

			lesson.SetType(lessonTypeResult.Value);

			await _unitOfWork.BeginTransactionAsync(cancellationToken);

			await _unitOfWork.SaveChangesAsync(cancellationToken);

			await _unitOfWork.CommitAsync(cancellationToken);

			return Result.Success();
		}
		catch (OperationCanceledException)
		{
			_logger.LogInformation("Запрос {request} был отменён", nameof(CreateLessonCommand));
			return Result.Failure("Запрос отменён");
		}
		catch (Exception ex)
		{
			_logger.LogError("{Exception}", ex.Message + ex.InnerException?.Message);

			await _unitOfWork.RollbackAsync();

			return Result.Failure("Упс! При создании занятия произошла ошибка.");
		}
	}
}
