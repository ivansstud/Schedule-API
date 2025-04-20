using Ardalis.Specification;

namespace ScheduleProject.Core.Specifications.Lesson;

public class LessonByScheduleSpec : SpecificationBase<Entities.Lesson>, ISingleResultSpecification<Entities.Lesson>
{
	public LessonByScheduleSpec(long scheduleId, bool isTracking = true, bool includeDeleted = false) : base(isTracking, includeDeleted)
	{
		Query
			.Where(x => x.ScheduleId == scheduleId);
	}
}
