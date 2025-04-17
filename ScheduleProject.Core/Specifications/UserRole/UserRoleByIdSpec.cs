using Ardalis.Specification;

namespace ScheduleProject.Core.Specifications.UserRole;

public class UserRoleByIdSpec : Specification<Entities.UserRole>, ISingleResultSpecification<Entities.UserRole>
{
	public UserRoleByIdSpec(long roleId, bool isTracking = true, bool includeDeleted = false)
	{
		Query
			.Where(x => x.Id == roleId);

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
