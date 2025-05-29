using CSharpFunctionalExtensions;
using ScheduleProject.Core.Entities.Abstractions;

namespace ScheduleProject.Core.Entities;

public class UserRole : EntityBase
{
	public const int MaxNameLength = 50;

	private readonly List<AppUser> _users = [];

	private UserRole() { }

	private UserRole(long id, string name, DateTime createDate) : base(id)
	{
		Name = name;
		CreatedAt = createDate;
	}

	public string Name { get; private set; } = null!;
	public IReadOnlyList<AppUser> Users => _users;

	public static UserRole Create(long id, string name, DateTime createDate)
	{
		ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);

		var result = new UserRole(id, name, createDate);
		return result;
	}
}
