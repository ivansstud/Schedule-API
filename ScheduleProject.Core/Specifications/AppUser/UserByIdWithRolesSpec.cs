using Ardalis.Specification;

namespace ScheduleProject.Core.Specifications.AppUser;

public class UserByIdWithRolesSpec : Specification<Entities.AppUser>, ISingleResultSpecification<Entities.AppUser>
{
	public UserByIdWithRolesSpec(long userId, bool isTracking = true, bool includeDeleted = false)
	{
		Query
			.Include(x => x.Roles)
			.Where(x => x.Id == userId);

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
