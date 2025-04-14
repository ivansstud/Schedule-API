namespace ScheduleProject.Core.Entities.Abstractions;

public interface IAuditable
{
	public DateTime CreatedAt { get; }
	public DateTime? ModifiedAt { get; }
}
