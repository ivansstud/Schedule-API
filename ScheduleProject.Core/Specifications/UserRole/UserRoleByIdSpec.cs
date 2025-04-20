using Ardalis.Specification;

namespace ScheduleProject.Core.Specifications.UserRole;

public class UserRoleByIdSpec : SpecificationBase<Entities.UserRole>, ISingleResultSpecification<Entities.UserRole>
{
	public UserRoleByIdSpec(long roleId, bool isTracking = true, bool includeDeleted = false) : base(isTracking, includeDeleted)
	{
		Query
			.Where(x => x.Id == roleId);
	}
}
