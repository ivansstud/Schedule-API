using Ardalis.Specification;
using ScheduleProject.Core.Entities;

namespace ScheduleProject.Core.Specifications.Schedule;

public class ScheduleByIdWithMembersAndLessonsSpec : SpecificationBase<Entities.Schedule>, ISingleResultSpecification<Entities.Schedule>
{
	public ScheduleByIdWithMembersAndLessonsSpec(long id, bool isTracking = true, bool includeDeleted = false) : base(isTracking, includeDeleted)
	{
		Query
			.Where(x => x.Id == id)
			.Include(x => x.Lessons)
			.Include(x => x.Members)
			.ThenInclude(x => x.User);
	}
}
