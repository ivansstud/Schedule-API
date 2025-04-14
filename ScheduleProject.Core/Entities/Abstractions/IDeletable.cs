namespace ScheduleProject.Core.Entities.Abstractions;

public interface IDeletable
{
	protected bool IsDeleted { get; }
}
