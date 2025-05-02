using CSharpFunctionalExtensions;
using MediatR;
using ScheduleProject.Core.Entities.Enums;

namespace ScheduleProject.Application.Requests.Schedules;

public class CreateScheduleCommand : IRequest<Result>
{
	public CreateScheduleCommand(string name, string? description, int type, int weeksType, long? institutionId)
	{
		Name = name;
		Description = description;
		Type = type;
		WeeksType = weeksType;
		InstitutionId = institutionId;
	}

	public string Name { get; private set; }
	public string? Description { get; private set; }
	public int Type { get; private set; }
	public int WeeksType { get; private set; }
	public long? InstitutionId { get; private set; }
}
