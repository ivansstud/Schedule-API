using CSharpFunctionalExtensions;
using MediatR;
namespace ScheduleProject.Application.Requests.Lessons;

public class UpdateLessonCommand : IRequest<Result>
{
	public long Id { get; set; }
	public required string Name { get; set; }
	public required string LessonType { get; set; }
	public string? Description { get; set; }
	public string? TeacherName { get; set; }
	public string? Audience { get; set; }
	public TimeOnly StartTime { get; set; }
	public TimeOnly EndTime { get; set; }
}