using CSharpFunctionalExtensions;
using MediatR;
using ScheduleProject.Application.DTOs.Lesson;

namespace ScheduleProject.Application.Requests.Lessons;

public class GetLessonsByScheduleRequest : IRequest<Result<LessonByScheduleDto[]>>
{
    public GetLessonsByScheduleRequest(long scheduleId)
    {
        ScheduleId = scheduleId;
    }

    public long ScheduleId { get; set; }
}
