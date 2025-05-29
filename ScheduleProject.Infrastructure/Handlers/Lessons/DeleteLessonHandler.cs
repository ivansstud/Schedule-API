using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ScheduleProject.Application.Requests.Lessons;
using ScheduleProject.Core.Entities;
using ScheduleProject.Infrastructure.DAL.Services;

namespace ScheduleProject.Infrastructure.Handlers.Lessons;

public class DeleteLessonHandler : IRequestHandler<DeleteLessonCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteLessonHandler> _logger;
    
    public DeleteLessonHandler(IUnitOfWork unitOfWork, ILogger<DeleteLessonHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    
    public async Task<Result> Handle(DeleteLessonCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var lesson = await _unitOfWork.Db
                .Set<Lesson>()
                .Where(x => x.Id == request.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (lesson is null)
            {
                _logger.LogWarning("Попытка удалить Lesson с несуществующим Id: {Id}", request.Id);
                return Result.Failure($"Такого занятия не найдено");
            }

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            lesson.SetDeleted(true);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
                        
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError("{Exception}", ex.Message + ex.InnerException?.Message);
            
            await _unitOfWork.RollbackAsync();
            
            return Result.Failure("Упс! При удалении занятия произошла ошибка.");
        }
    }
}