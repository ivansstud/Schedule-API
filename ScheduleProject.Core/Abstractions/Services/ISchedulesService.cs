using Ardalis.Specification;
using CSharpFunctionalExtensions;
using ScheduleProject.Core.Dtos.Schedule;
using ScheduleProject.Core.Entities;

namespace ScheduleProject.Core.Abstractions.Services;

public interface ISchedulesService
{
	Task<Result<long>> CreateAsync(CreateScheduleDto createDto, CancellationToken cancellationToken = default);

	Task<Result<List<Schedule>>> GetListBySpecification(ISpecification<Schedule> specification, CancellationToken cancellationToken = default);

	Task<Result<Schedule?>> GetSingleBySpecificationAsync(ISingleResultSpecification<Schedule> specification, CancellationToken cancellationToken = default);
}