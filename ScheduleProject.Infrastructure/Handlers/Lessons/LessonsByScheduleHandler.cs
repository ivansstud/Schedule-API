using MediatR;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ScheduleProject.Application.DTOs.Lesson;
using ScheduleProject.Application.Requests.Lessons;
using ScheduleProject.Core.Entities;
using ScheduleProject.Infrastructure.DAL.Services;
using ScheduleProject.Core.Entities.ValueObjects;
using ScheduleProject.Application.DTOs.ValueObjects;

namespace ScheduleProject.Infrastructure.Handlers.Lessons;

public class LessonsByScheduleHandler : IRequestHandler<LessonsByScheduleRequest, Result<LessonByScheduleDto[]>>
{
    private readonly ILogger<LessonsByScheduleHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    
    public LessonsByScheduleHandler(ILogger<LessonsByScheduleHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<LessonByScheduleDto[]>> Handle(LessonsByScheduleRequest request, CancellationToken cancellationToken)
    {
        try
        {
			var lessons = await _unitOfWork.Db
                .Set<Lesson>()
                .AsNoTracking()
                .Where(x => x.ScheduleId == request.ScheduleId
                         && x.IsDeleted == false)
                .Select(x => new LessonByScheduleDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Day = x.Day,
                    EndTime = x.EndTime,
                    Audience = x.Audience,
                    StartTime = x.StartTime,
                    Description = x.Description,
                    TeacherName = x.TeacherName,
                    LessonType = x.LessonType.Name,
                    ScheduleWeeksType = x.ScheduleWeeksType
                })
                .ToArrayAsync(cancellationToken);
            
            return lessons;
        }
        catch (Exception ex)
        {
            _logger.LogError("{Exception}:", ex.Message + ex.InnerException?.Message);
            
            return Result.Failure<LessonByScheduleDto[]>("Упс! При получении занятий произошла ошибка.");
        }
    }
}