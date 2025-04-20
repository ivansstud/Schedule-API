using Ardalis.Specification;
using CSharpFunctionalExtensions;
using ScheduleProject.Core.Dtos.Lesson;
using ScheduleProject.Core.Entities;

namespace ScheduleProject.Core.Abstractions.Services;

public interface ILessonsService
{
	Task<Result<long>> CreateAsync(CreateLessonDto createDto, CancellationToken cancellationToken = default);

	Task<Result> DeleteAsync(long lessonId, CancellationToken cancellationToken = default);

	Task<Result<List<Lesson>>> GetListBySpecificationAsync(ISpecification<Lesson> specification, CancellationToken cancellationToken = default);

	Task<Result<Lesson?>> GetSingleBySpecificationAsync(ISingleResultSpecification<Lesson> specification, CancellationToken cancellationToken = default);
}