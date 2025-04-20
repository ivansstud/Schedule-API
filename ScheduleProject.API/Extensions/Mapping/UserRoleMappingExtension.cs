using ScheduleProject.API.Dtos.Responce.UserRole;
using ScheduleProject.Core.Entities;

namespace ScheduleProject.API.Extensions.Mapping;

public static class UserRoleMappingExtension
{
	public static UserRoleResponce MapToResponce(this UserRole role)
	{
		return new UserRoleResponce
		{
			Id = role.Id,
			Name = role.Name,
			IsDeleted = role.IsDeleted
		};
	}
}
