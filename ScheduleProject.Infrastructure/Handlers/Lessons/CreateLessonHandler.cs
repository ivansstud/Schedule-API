using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.Logging;
using ScheduleProject.Application.Requests.Lessons;
using ScheduleProject.Core.Entities;
using ScheduleProject.Core.Entities.ValueObjects;
using ScheduleProject.Infrastructure.DAL.Services;

namespace ScheduleProject.Infrastructure.Handlers.Lessons;

public class CreateLessonHandler : IRequestHandler<CreateLessonCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateLessonHandler> _logger;
    
    public CreateLessonHandler(IUnitOfWork unitOfWork, ILogger<CreateLessonHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    
    public async Task<Result> Handle(CreateLessonCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var lessonTypeResult = LessonType.Create(request.Type);

            if (!lessonTypeResult.TryGetValue(out var lessonType))
            {
                return Result.Failure(lessonTypeResult.Error);
            }

            var scheduleWeeksType = ScheduleWeeksType.FromValue(request.ScheduleWeeksType);
            var day = Day.FromValue(request.Day);

            if (scheduleWeeksType is null || day is null)
            {
				_logger.LogError("Неудачная попытка создания расписания для ScheduleId: {ScheduleId}, Day: {Day}, WeeksType: {WeeksType}",
                    request.ScheduleId, request.Day, request.ScheduleWeeksType);
				return Result.Failure("Упс! Не удалось создать занятие.");
			}

			var newLessonResult = Lesson.Create(
                name: request.Name,
                description: request.Description,
                teacherName: request.TeacherName,
                audience: request.Audience,
                lessonType: lessonType,
                startTime: request.StartTime,
                endTime: request.EndTime,
                scheduleWeeksType: scheduleWeeksType,
                day: day,
                scheduleId: request.ScheduleId
            );

            if (!newLessonResult.TryGetValue(out var lesson))
            {
                return Result.Failure(newLessonResult.Error);
            }
            
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            await _unitOfWork.Db.AddAsync(lesson, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
            
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError("{Exception}", ex.Message + ex.InnerException?.Message);
            
            await _unitOfWork.RollbackAsync();
            
            return Result.Failure("Упс! При создании занятия произошла ошибка.");
        }
    }
}