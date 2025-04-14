using ScheduleProject.Core.Entities.Abstractions;

namespace ScheduleProject.Core.Entities;

public class UserRole : EntityBase
{
	public const int MaxNameLength = 50;

	private readonly List<AppUser> _users = [];

	private UserRole(long id, string name) : base(id)
	{
		Name = name;
	}

	public string Name { get; private set; } = null!;
	public IReadOnlyList<AppUser> Users => _users;

	public static UserRole Create(long id, string name)
	{
		var result = new UserRole(id, name);
		return result;
	}
}
