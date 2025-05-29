using CSharpFunctionalExtensions;
using MediatR;

namespace ScheduleProject.Application.Requests.Institusions;

public class CreateInstitusionCammand : IRequest<Result>
{
	public string Name { get; set; } = null!;
	public string ShortName { get; set; } = null!;
	public string? Description { get; set; }
}
