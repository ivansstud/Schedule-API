using CSharpFunctionalExtensions;

#pragma warning disable CS8618

namespace ScheduleProject.Core.Entities;

public class TelegramUser : Entity
{
	public const int MaxFirstNameLength = 64;
	public const int MaxLastNameLength = 64;
	public const int MaxUserNameLength = 32;

	private readonly List<ScheduleMember> _scheduleMemberships = [];

	private TelegramUser() { } // Для EF Core

	private TelegramUser(long id, string? firstName, string? lastName, string userName, string? photoUrl) : base(id)
	{
		FirstName = firstName;
		LastName = lastName;
		UserName = userName;
		PhotoUrl = photoUrl;
	}

	public string? FirstName { get; private set; }
	public string? LastName { get; private set; }
	public string UserName { get; private set; }
	public string? PhotoUrl { get; private set; }
	public IReadOnlyList<ScheduleMember> ScheduleMemberships => _scheduleMemberships;

	public static Result<TelegramUser> Create(long id, string? firstName, string? lastName, string userName, string? photoUrl)
	{
		if (firstName?.Length > MaxFirstNameLength)
		{
			return Result.Failure<TelegramUser>($"Имя не может быть длиннее {MaxFirstNameLength} символов");
		}
		if (lastName?.Length > MaxLastNameLength)
		{
			return Result.Failure<TelegramUser>($"Фамилия не может быть длиннее {MaxLastNameLength} символов");
		}
		if (userName?.Length > MaxUserNameLength)
		{
			return Result.Failure<TelegramUser>($"Telegram @username не может быть длиннее {MaxUserNameLength} символов");
		}
		if (string.IsNullOrEmpty(userName))
		{
			return Result.Failure<TelegramUser>($"Telegram @username не может быть пустым");
		}

		return new TelegramUser(id, firstName, lastName, userName, photoUrl);
	}
}
