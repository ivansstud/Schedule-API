namespace ScheduleProject.Core.Entities.Abstractions;

public interface IDeletable
{
	bool IsDeleted { get; }
	DateTime? DeletionDate { get; }
}
