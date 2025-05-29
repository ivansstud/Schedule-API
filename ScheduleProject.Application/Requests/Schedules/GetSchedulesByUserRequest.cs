using CSharpFunctionalExtensions;
using MediatR;
using ScheduleProject.Application.DTOs.Schedules;

namespace ScheduleProject.Application.Requests.Schedules;

public class GetSchedulesByUserRequest : IRequest<Result<ScheduleByUserDto[]>>
{
	public long UserId { get; set; }

	public GetSchedulesByUserRequest(long userId)
	{
		UserId = userId;
	}
}
