using CSharpFunctionalExtensions;
using MediatR;
using ScheduleProject.Application.DTOs.Institusions;

namespace ScheduleProject.Application.Requests.Institusions;

public class GetInstituisonsRequest : IRequest<Result<InstitusionShortInfoDto[]>>
{
	public int Take { get; set; }
	public int Skip { get; set; } = 0;
	public string NameSubstring { get; set; } = null!;
	public bool GetAll { get; set; } = false;
}
