using Ardalis.Specification;
using CSharpFunctionalExtensions;
using ScheduleProject.Core.Dtos.Schedule;
using ScheduleProject.Core.Entities;

namespace ScheduleProject.Core.Abstractions.Services;

public interface IScheduleService
{
	Task<Result<long>> CreateAsync(CreateScheduleDto createDto, CancellationToken cancellationToken = default);

	Task<Result<List<Schedule>>> GetAllBySpecification(ISpecification<Schedule> specification, CancellationToken cancellationToken = default);

	Task<Result<Schedule?>> GetSingleBySpecificationAsync(ISingleResultSpecification<Schedule> specification, CancellationToken cancellationToken = default);
}