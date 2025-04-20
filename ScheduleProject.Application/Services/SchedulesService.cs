using Ardalis.Specification;
using CSharpFunctionalExtensions;
using ScheduleProject.Core.Abstractions.DAL;
using ScheduleProject.Core.Abstractions.Services;
using ScheduleProject.Core.Dtos.Schedule;
using ScheduleProject.Core.Entities;
using ScheduleProject.Core.Entities.Enums;

namespace ScheduleProject.Application.Services;

public class SchedulesService : ISchedulesService
{
	private readonly IRepository<Schedule> _scheduleRepository;

	public SchedulesService(IRepository<Schedule> scheduleRepository)
	{
		_scheduleRepository = scheduleRepository;
	}

	public async Task<Result<long>> CreateAsync(CreateScheduleDto createDto, CancellationToken cancellationToken = default)
	{
		var scheduleResult = Schedule.Create(createDto.Name, createDto.Description, createDto.Type, createDto.WeeksType, createDto.InstitusionId);

		if (scheduleResult.IsFailure)
		{
			return Result.Failure<long>(scheduleResult.Error);
		}

		var newSchedule = await _scheduleRepository.AddAsync(scheduleResult.Value, cancellationToken);
		var scheduleMemberResult = ScheduleMember.Create(newSchedule.Id, createDto.UserId, ScheduleRole.Creator);

		if (!scheduleMemberResult.TryGetValue(out ScheduleMember? creator))
		{
			return Result.Failure<long>(scheduleMemberResult.Error);
		}

		newSchedule.AddMember(creator);
		await _scheduleRepository.CommitAsync(cancellationToken);

		return newSchedule.Id;
	}

	public async Task<Result<List<Schedule>>> GetListBySpecificationAsync(ISpecification<Schedule> specification, CancellationToken cancellationToken = default)
	{
		return await _scheduleRepository.GetListBySpecificationAsync(specification, cancellationToken);
	}

	public async Task<Result<Schedule?>> GetSingleBySpecificationAsync(ISingleResultSpecification<Schedule> specification, CancellationToken cancellationToken = default)
	{
		return await _scheduleRepository.GetSingleBySpecificationAsync(specification, cancellationToken);
	}
}
