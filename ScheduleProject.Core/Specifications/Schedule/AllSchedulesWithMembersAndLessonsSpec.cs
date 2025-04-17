using Ardalis.Specification;
using ScheduleProject.Core.Entities;

namespace ScheduleProject.Core.Specifications.Schedule;

public class AllSchedulesWithMembersAndLessonsSpec : Specification<Entities.Schedule>
{
	public AllSchedulesWithMembersAndLessonsSpec(bool isTracking = true, bool includeDeleted = false)
	{
		Query
			.Include(x => x.Lessons)
			.Include(x => x.Members)
			.ThenInclude(x => x.User);

		if (!includeDeleted)
		{
			Query.Where(x => !x.IsDeleted);
		}
		if (!isTracking)
		{
			Query.AsNoTracking();
		}
	}
}
