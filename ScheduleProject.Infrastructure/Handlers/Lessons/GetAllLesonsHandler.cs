using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ScheduleProject.Application.DTOs.Lesson;
using ScheduleProject.Application.Requests.Lessons;
using ScheduleProject.Core.Entities;
using ScheduleProject.Infrastructure.DAL.Services;
using ScheduleProject.Infrastructure.Extensions.Mapping;

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
				.Select(x => x.MapToFullDto())
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
