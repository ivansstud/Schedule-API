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

	public const int MinFirstNameLength = 1;
	public const int MinLastNameLength = 1;
	public const int MinLoginLength = 6;
	public const int MinPasswordLength = 6;

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
	public long? AuthTokenId { get; private set; } = null;
	public AuthToken? AuthToken { get; private set; } = null;
	public IReadOnlyList<ScheduleMember> ScheduleMemberships => _scheduleMemberships;
	public IReadOnlyList<UserRole> Roles => _roles;

	public static Result<AppUser> Create(string login, string passwordHash, string firstName, string lastName)
	{
		Result[] validationResults = [
			ValidateFirstName(firstName),
			ValidateLastName(lastName),
			ValidateLogin(login),
		];

		if (validationResults.FirstOrDefault(x => x.IsFailure) is { } failure)
		{
			return Result.Failure<AppUser>(failure.Error);
		}
		
		var result = new AppUser(login, passwordHash, firstName, lastName);
		return result;
	}

	public void SetAuthToken(AuthToken token)
	{
		AuthToken = token;
	}

	public Result SetFirstName(string firstName)
	{
		if (ValidateFirstName(firstName).TryGetError(out var error))
		{
			return Result.Failure(error);
		}

		FirstName = firstName;
		return Result.Success();
	}

	public Result SetLastName(string lastName)
	{
		if (ValidateLastName(lastName).TryGetError(out var error))
		{
			return Result.Failure(error);
		}

		LastName = lastName;
		return Result.Success();
	}

	public Result SetLogin(string login)
	{
		if (ValidateLogin(login).TryGetError(out var error))
		{
			return Result.Failure(error);
		}

		Login = login;
		return Result.Success();
	}

	public void AddRole(UserRole role)
	{
		if (!_roles.Any(x => x.Id == role.Id))
		{
			_roles.Add(role);
		}
	}

	public void RemoveRole(UserRole role)
	{
		if (_roles.Any(x => x.Id == role.Id))
		{
			_roles.Remove(role);
		}
	}

	public void AddMemberships(ScheduleMember member)
	{
		if (!_scheduleMemberships.Any(x => x.Id == member.Id))
		{
			_scheduleMemberships.Add(member);
		}
	}

	public void RemoveMemberships(ScheduleMember member)
	{
		if (_scheduleMemberships.Any(x => x.Id == member.Id))
		{
			_scheduleMemberships.Remove(member);
		}
	}

	private static Result ValidateFirstName(string firstName)
	{
		if (firstName.Length > MaxFirstNameLength || firstName.Length < MinFirstNameLength)
		{
			return Result.Failure($"Имя должно содержать от {MinFirstNameLength} до {MaxFirstNameLength} символов");
		}

		return Result.Success();
	}

	private static Result ValidateLastName(string lastName)
	{
		if (lastName.Length > MaxLastNameLength || lastName.Length < MinLastNameLength)
		{
			return Result.Failure($"Фамилия должна содержать от {MinLastNameLength} до {MaxLastNameLength} символов");
		}

		return Result.Success();
	}

	private static Result ValidateLogin(string login)
	{
		if (login.Length > MaxLoginLength || login.Length < MinLoginLength)
		{
			return Result.Failure($"Логин должен содержать от {MinLoginLength} до {MaxLoginLength} символов");
		}

		return Result.Success();
	}
}
