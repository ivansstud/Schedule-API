using ScheduleProject.Core.Entities;
using ScheduleProject.API.Dtos.Responce.Schedule;
using ScheduleProject.API.Helpers;
using System.Data;
using ScheduleProject.Core.Entities.Enums;

namespace ScheduleProject.API.Extensions.Mapping;

public static class ScheduleMappingExtension
{
	public static ScheduleFullInfoResponce MapToFullInfoResponse(this Schedule schedule)
	{
		return new ScheduleFullInfoResponce
		{
			Name = schedule.Name,
			Description = schedule.Description,
			Type = schedule.Type,
			WeeksType = schedule.WeeksType,
			Members = schedule.Members.Select(m => new ScheduleMemberResponce
			{
				FirstName = m.User.FirstName,
				LastName = m.User.LastName,
				Role = m.Role
			}).ToList(),
			Lessons = schedule.Lessons.Select(l => new LessonResponce
			{
				Name = l.Name,
				Description = l.Description,
				Audience = l.Audience,
				Type = l.LessonType.Value,
				StartTime = l.StartTime,
				EndTime = l.EndTime,
				WeeksType = l.SheduleWeeksType,
				Day = l.DayOfWeek
			}).ToList()
		};
	}

	public static ScheduleInfoResponce MapToInfoResponce(this Schedule schedule)
	{
		return new ScheduleInfoResponce
		{
			Name = schedule.Name,
			Description = schedule.Description,
			Type = schedule.Type,
			WeeksType = schedule.WeeksType,
			InstitutionId = schedule.InstitutionId
		};
	}

	public static SchedulePreviewResponce MapToPreviewResponse(this Schedule schedule, long userId)
	{
		var role = schedule.Members.First(x => x.UserId == userId).Role;
		var roleName = ScheduleRolesHelper.GetName(role);
		var typeName = ScheduleTypesHelper.GetName(schedule.Type);

		return new SchedulePreviewResponce
		{
			Id = schedule.Id,
			Name = schedule.Name,
			RoleName = roleName,
			TypeName = typeName,
		};
	}
}
