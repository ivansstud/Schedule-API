using Ardalis.Specification;
using ScheduleProject.Core.Entities;

namespace ScheduleProject.Core.Specifications.Schedule;

public class SchedulesWithMembersAndLessonsSpec : SpecificationBase<Entities.Schedule>
{
	public SchedulesWithMembersAndLessonsSpec(bool isTracking = true, bool includeDeleted = false) : base(isTracking, includeDeleted)
	{
		Query
			.Include(x => x.Lessons)
			.Include(x => x.Members)
			.ThenInclude(x => x.User);
	}
}
