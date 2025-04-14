using CSharpFunctionalExtensions;
using ScheduleProject.Core.Entities.Abstractions;


#pragma warning disable CS8618

namespace ScheduleProject.Core.Entities;

public class AppUser : EntityBase
{
	public const int MaxFirstNameLength = 24;
	public const int MaxLastNameLength = 24;
	public const int MaxLoginLength = 24;
	public const int MaxPasswordLength = 24;
	public const int MaxHashedPasswordLength = 128;

	private readonly List<ScheduleMember> _scheduleMemberships = [];
	private readonly List<UserRole> _roles = [];

	private AppUser() { } // Для EF Core

	private AppUser(string login, string passwordHash, string firstName, string lastName)
	{
		FirstName = firstName;
		LastName = lastName;
		Login = login;
		HashedPassword = passwordHash;
	}

	public string FirstName { get; private set; }
	public string LastName { get; private set; }
	public string Login { get; private set; }
	public string HashedPassword { get; private set; }
	public long? AuthTokenId { get; private set; }
	public AuthToken? AuthToken { get; private set; }
	public IReadOnlyList<ScheduleMember> ScheduleMemberships => _scheduleMemberships;
	public IReadOnlyList<UserRole> Roles => _roles;

	public static Result<AppUser> Create(string login, string passwordHash, string firstName, string lastName)
	{
		if (firstName.Length > MaxFirstNameLength)
		{
			return Result.Failure<AppUser>($"Имя не может быть длиннее {MaxFirstNameLength} символов");
		}
		if (lastName.Length > MaxLastNameLength)
		{
			return Result.Failure<AppUser>($"Фамилия не может быть длиннее {MaxLastNameLength} символов");
		}
		if (login.Length > MaxLoginLength)
		{
			return Result.Failure<AppUser>($"Логин не может быть длиннее {MaxLoginLength} символов");
		}
		
		var result = new AppUser(login, passwordHash, firstName, lastName);
		return result;
	}

	public void SetAuthToken(AuthToken token)
	{
		AuthToken = token;
	}

	public void AddRole(UserRole role)
	{
		if (!_roles.Any(x => x.Id == role.Id))
		{
			_roles.Add(role);
		}
	}

	public void AddMemberships(ScheduleMember member)
	{
		if (!_scheduleMemberships.Any(x => x.Id == member.Id))
		{
			_scheduleMemberships.Add(member);
		}
	}
}
