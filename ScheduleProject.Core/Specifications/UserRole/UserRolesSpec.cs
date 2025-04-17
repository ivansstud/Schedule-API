using Ardalis.Specification;

namespace ScheduleProject.Core.Specifications.UserRole;

public class UserRolesSpec : Specification<Entities.UserRole>
{
	public UserRolesSpec(bool isTracking = true, bool includeDeleted = false)
	{
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
