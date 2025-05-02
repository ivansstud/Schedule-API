using System.Reflection;

namespace ScheduleProject.Core.Entities.Enums;

public static class AppRoles
{
	public const string DomainUser = nameof(DomainUser);
	public const string InstitusionAdder = nameof(InstitusionAdder);
	public const string InstitusionRemover = nameof(InstitusionRemover);
	public const string Administrator = nameof(Administrator);

	public static IReadOnlyList<string> GetRoles()
	{
		return typeof(AppRoles)
			.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
			.Where(field => field.IsLiteral && !field.IsInitOnly && field.FieldType == typeof(string))
			.Select(field => (string?)field.GetValue(null))
			.ToArray()!;
	}
}
