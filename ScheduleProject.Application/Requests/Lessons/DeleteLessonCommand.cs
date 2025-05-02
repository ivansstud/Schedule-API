using CSharpFunctionalExtensions;
using MediatR;

namespace ScheduleProject.Application.Requests.Lessons;

public class DeleteLessonCommand : IRequest<Result>
{
    public DeleteLessonCommand(long id)
    {
        Id = id;
    }

    public long Id { get; }
}