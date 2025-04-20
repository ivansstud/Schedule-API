using Ardalis.Specification;
using ScheduleProject.Core.Entities.Abstractions;

namespace ScheduleProject.Core.Specifications;

public class SpecificationBase<T> : Specification<T> where T : EntityBase
{
	public SpecificationBase(bool isTracking = true, bool includeDeleted = false)
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
