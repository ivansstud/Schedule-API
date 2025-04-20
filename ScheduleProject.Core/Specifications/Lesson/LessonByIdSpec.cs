using Ardalis.Specification;

namespace ScheduleProject.Core.Specifications.Lesson;

public class LessonByIdSpec : SpecificationBase<Entities.Lesson>, ISingleResultSpecification<Entities.Lesson>
{
	public LessonByIdSpec(long lessonId, bool isTracking = true, bool includeDeleted = false) : base(isTracking, includeDeleted)
	{
		Query
			.Where(x => x.Id == lessonId);
	}
}
