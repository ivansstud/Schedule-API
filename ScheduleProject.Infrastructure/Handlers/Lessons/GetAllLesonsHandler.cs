using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ScheduleProject.Application.DTOs.Lesson;
using ScheduleProject.Application.Requests.Lessons;
using ScheduleProject.Core.Entities;
using ScheduleProject.Infrastructure.DAL.Services;

namespace ScheduleProject.Infrastructure.Handlers.Lessons;

public class GetAllLesonsHandler : IRequestHandler<GetAllLessonsRequest, Result<LessonDto[]>>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ILogger<GetAllLesonsHandler> _logger;

	public GetAllLesonsHandler(IUnitOfWork unitOfWork, ILogger<GetAllLesonsHandler> logger)
	{
		_unitOfWork = unitOfWork;
		_logger = logger;
	}

	public async Task<Result<LessonDto[]>> Handle(GetAllLessonsRequest request, CancellationToken cancellationToken)
	{
		try
		{
			 var lessons = await _unitOfWork.Db
				.Set<Lesson>()
				.AsNoTracking()
				.Select(x => new LessonDto
				{
					Id = x.Id,
					Name = x.Name,
					Audience = x.Audience,
					TeacherName = x.TeacherName,
					Day = x.Day,
					Description = x.Description,
					EndTime = x.EndTime,
					StartTime = x.StartTime,
					ScheduleId = x.ScheduleId,
					ScheduleWeeksType = x.ScheduleWeeksType,
					LessonType = x.LessonType,
					IsDeleted = x.IsDeleted
				})
				.ToArrayAsync(cancellationToken);

			return lessons;
		}
		catch (Exception ex)
		{
			_logger.LogError("{Exception}", ex.Message + ex.InnerException?.Message);

			return Result.Failure<LessonDto[]>("Упс! получении данных о занятиях произошла ошибка.");
		}
	}
}
