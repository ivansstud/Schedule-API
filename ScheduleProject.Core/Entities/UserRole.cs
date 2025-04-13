using CSharpFunctionalExtensions;

namespace ScheduleProject.Core.Entities;

public class UserRole : Entity
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
		return new(id, name);
	}
}
