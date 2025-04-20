using Ardalis.Specification;
using ScheduleProject.Core.Entities;

namespace ScheduleProject.Core.Specifications.Schedule;

public class SchedulesByUserSpec : SpecificationBase<Entities.Schedule>
{
	public SchedulesByUserSpec(long userId, bool isTracking = true, bool includeDeleted = false) : base(isTracking, includeDeleted)
	{
		Query
			.Include(x => x.Members)
			.Where(x => x.Members.Any(x => x.UserId == userId));
	}
}
