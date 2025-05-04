using CSharpFunctionalExtensions;
using MediatR;

namespace ScheduleProject.Application.Requests.Schedules;

public class DeleteScheduleCommand : IRequest<Result>
{
	public DeleteScheduleCommand(long id)
	{
		Id = id;
	}

	public long Id { get; set; }
}
