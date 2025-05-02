using CSharpFunctionalExtensions;
using MediatR;
using ScheduleProject.Application.DTOs.Lesson;
using ScheduleProject.Core.Entities;

namespace ScheduleProject.Application.Requests.Lessons;

public class GetAllLessonsRequest : IRequest<Result<LessonDto[]>>
{
}
