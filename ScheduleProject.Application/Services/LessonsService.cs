using Ardalis.Specification;
using CSharpFunctionalExtensions;
using ScheduleProject.Core.Abstractions.DAL;
using ScheduleProject.Core.Abstractions.Services;
using ScheduleProject.Core.Dtos.Lesson;
using ScheduleProject.Core.Entities;
using ScheduleProject.Core.Entities.ValueObjects;
using ScheduleProject.Core.Specifications.Lesson;

namespace ScheduleProject.Application.Services;

public class LessonsService : ILessonsService
{
	private readonly IRepository<Lesson> _lessonsRepository;

	public LessonsService(IRepository<Lesson> lessonsRepository)
	{
		_lessonsRepository = lessonsRepository;
	}

	public async Task<Result<long>> CreateAsync(CreateLessonDto createDto, CancellationToken cancellationToken = default)
	{
		var lessonTypeResult = LessonType.Create(createDto.LessonType);

		if (!lessonTypeResult.TryGetValue(out var lessonType))
		{
			return Result.Failure<long>(lessonTypeResult.Error);
		}

		var lessonResult = Lesson.Create(
			name: createDto.Name,
			description: createDto.Description,
			teacherName: createDto.TeacherName,
			audience: createDto.Audience,
			lessonType: lessonType,
			startTime: createDto.StartTime,
			endTime: createDto.EndTime,
			scheduleWeeksType: createDto.SheduleWeeksType,
			dayOfWeek: createDto.Day,
			scheduleId: createDto.ScheduleId
		);

		if (!lessonResult.TryGetValue(out var lesson))
		{
			return Result.Failure<long>(lessonResult.Error);
		}

		await _lessonsRepository.AddAsync(lesson, cancellationToken);
		await _lessonsRepository.CommitAsync(cancellationToken);

		return lesson.Id;
	}

	public async Task<Result> DeleteAsync(long lessonId, CancellationToken cancellationToken = default)
	{
		var lessonSpecification = new LessonByIdSpec(lessonId, isTracking: true);
		var lesson = await _lessonsRepository.GetSingleBySpecificationAsync(lessonSpecification, cancellationToken);

		if (lesson is null)
		{
			return Result.Failure($"Занятия с id {lessonId} не существует");
		}

		lesson.SetDeleted(true);
		await _lessonsRepository.CommitAsync(cancellationToken);

		return Result.Success();
	}

	public async Task<Result<List<Lesson>>> GetListBySpecificationAsync(ISpecification<Lesson> specification, CancellationToken cancellationToken = default)
	{
		return await _lessonsRepository.GetListBySpecificationAsync(specification, cancellationToken);
	}

	public async Task<Result<Lesson?>> GetSingleBySpecificationAsync(ISingleResultSpecification<Lesson> specification, CancellationToken cancellationToken = default)
	{
		return await _lessonsRepository.GetSingleBySpecificationAsync(specification, cancellationToken);
	}
}
