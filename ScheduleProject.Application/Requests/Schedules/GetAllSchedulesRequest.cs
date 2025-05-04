using CSharpFunctionalExtensions;
using MediatR;
using ScheduleProject.Application.DTOs.Schedules;

namespace ScheduleProject.Application.Requests.Schedules;

public class GetAllSchedulesRequest : IRequest<Result<ScheduleDto[]>>
{
	public static readonly GetAllSchedulesRequest Instance = new();
}
