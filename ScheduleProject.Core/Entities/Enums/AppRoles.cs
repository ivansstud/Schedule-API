namespace ScheduleProject.Core.Entities.Enums;

public static class AppRoles
{
	public const string DomainUser = nameof(DomainUser);
	public const string InstitusionAdder = nameof(InstitusionAdder);
	public const string InstitusionRemover = nameof(InstitusionRemover);
	public const string Administrator = nameof(Administrator);

	public static IReadOnlyList<string> GetRoles()
	{
		return [DomainUser, InstitusionAdder, InstitusionRemover, Administrator];
	}
}
