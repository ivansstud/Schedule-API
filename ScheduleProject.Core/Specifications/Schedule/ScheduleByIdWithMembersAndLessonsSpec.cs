using Ardalis.Specification;
using ScheduleProject.Core.Entities;

namespace ScheduleProject.Core.Specifications.Schedule;

public class ScheduleByIdWithMembersAndLessonsSpec : Specification<Entities.Schedule>, ISingleResultSpecification<Entities.Schedule>
{
	public ScheduleByIdWithMembersAndLessonsSpec(long id, bool isTracking = true, bool includeDeleted = false)
	{
		Query
			.Where(x => x.Id == id)
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
