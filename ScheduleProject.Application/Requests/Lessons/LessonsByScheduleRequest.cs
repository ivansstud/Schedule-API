using CSharpFunctionalExtensions;
using MediatR;
using ScheduleProject.Application.DTOs.Lesson;

namespace ScheduleProject.Application.Requests.Lessons;

public class LessonsByScheduleRequest : IRequest<Result<LessonByScheduleDto[]>>
{
    public LessonsByScheduleRequest(long scheduleId)
    {
        ScheduleId = scheduleId;
    }

    public long ScheduleId { get; }
}
