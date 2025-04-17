using Ardalis.Specification;
using ScheduleProject.Core.Entities;

namespace ScheduleProject.Core.Specifications.Schedule;

public class SchedulesByUserSpec : Specification<Entities.Schedule>
{
	public SchedulesByUserSpec(long userId, bool isTracking = true, bool includeDeleted = false)
	{
		Query
			.Include(x => x.Members)
			.Where(x => x.Members.Any(x => x.UserId == userId));

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
