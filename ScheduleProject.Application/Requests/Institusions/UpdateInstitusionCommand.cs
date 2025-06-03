using CSharpFunctionalExtensions;
using MediatR;

namespace ScheduleProject.Application.Requests.Institusions;

public class UpdateInstitusionCommand : IRequest<Result>
{
	public long Id { get; set; }
	public required string Name { get; set; }
	public required string ShortName { get; set; }
	public string? Description { get; set; }
}