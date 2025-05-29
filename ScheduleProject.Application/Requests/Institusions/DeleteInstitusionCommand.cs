using CSharpFunctionalExtensions;
using MediatR;

namespace ScheduleProject.Application.Requests.Institusions;

public class DeleteInstitusionCommand : IRequest<Result>
{
	public long Id { get; set; }
}