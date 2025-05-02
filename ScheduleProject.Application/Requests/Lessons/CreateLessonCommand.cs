using CSharpFunctionalExtensions;
using MediatR;
using ScheduleProject.Core.Entities.Enums;

namespace ScheduleProject.Application.Requests.Lessons;

public class CreateLessonCommand : IRequest<Result>
{
    public long ScheduleId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? TeacherName { get; set; }
    public string? Audience { get; set; }
    public string Type { get; set; } = null!;
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public int ScheduleWeeksType { get; set; }
    public int Day { get; set; }
}