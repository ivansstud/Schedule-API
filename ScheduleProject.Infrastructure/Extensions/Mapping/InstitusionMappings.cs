using ScheduleProject.Application.DTOs.Institusions;
using ScheduleProject.Core.Entities;

namespace ScheduleProject.Infrastructure.Extensions.Mapping;

public static class InstitusionMappings
{
	public static InstitusionShortInfoDto MapToShortInfo(this Institution institution)
	{
		return new InstitusionShortInfoDto
		{
			Id = institution.Id,
			Name = institution.Name
		};
	}
}
