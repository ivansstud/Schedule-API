using Ardalis.Specification;

namespace ScheduleProject.Core.Specifications.UserRole;

public class UserRolesSpec : SpecificationBase<Entities.UserRole>
{
	public UserRolesSpec(bool isTracking = true, bool includeDeleted = false) : base(isTracking, includeDeleted)
	{
	}
}
