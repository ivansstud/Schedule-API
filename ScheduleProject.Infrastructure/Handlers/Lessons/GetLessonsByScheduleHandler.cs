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
using ScheduleProject.Infrastructure.Extensions.Mapping;

namespace ScheduleProject.Infrastructure.Handlers.Lessons;

public class GetLessonsByScheduleHandler : IRequestHandler<GetLessonsByScheduleRequest, Result<LessonByScheduleDto[]>>
{
    private readonly ILogger<GetLessonsByScheduleHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    
    public GetLessonsByScheduleHandler(ILogger<GetLessonsByScheduleHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<LessonByScheduleDto[]>> Handle(GetLessonsByScheduleRequest request, CancellationToken cancellationToken)
    {
        try
        {
			var lessons = await _unitOfWork.Db
                .Set<Lesson>()
                .AsNoTracking()
                .Where(x => x.ScheduleId == request.ScheduleId && x.IsDeleted == false)
                .Select(x => x.MapToByScheduleDto())
                .ToArrayAsync(cancellationToken);
            
            return lessons;
        }
        catch (Exception ex)
        {
            _logger.LogError("{Exception}", ex.Message + ex.InnerException?.Message);
            
            return Result.Failure<LessonByScheduleDto[]>("Упс! При получении занятий произошла ошибка.");
        }
    }
}