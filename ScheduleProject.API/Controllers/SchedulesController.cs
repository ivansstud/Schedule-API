using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScheduleProject.API.Dtos.Request;
using ScheduleProject.Core.Abstractions.Services;
using ScheduleProject.Core.Dtos.Schedule;
using ScheduleProject.Core.Entities.Enums;
using ScheduleProject.Core.Specifications;
using System.Diagnostics.Contracts;
using System.Security.Claims;
using System.Threading;

namespace ScheduleProject.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SchedulesController : ControllerBase
{
	private readonly IScheduleService _scheduleService;

	public SchedulesController(IScheduleService scheduleService)
	{
		_scheduleService = scheduleService;
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<ScheduleResponce>> GetById(long id, CancellationToken cancellationToken)
	{
		var specification = new ScheduleByIdWithMembersAndLessonsSpec(id, isTracking: false, includeDeleted: false);
		var result = await _scheduleService.GetSingleBySpecificationAsync(specification, cancellationToken);

		if (result.IsFailure)
		{
			return BadRequest();
		}

		var schedule = result.Value;

		if (schedule is null)
		{
			return NotFound();
		}

		return new ScheduleResponce
		(
			Name: schedule.Name,
			Description: schedule.Description,
			Type: schedule.Type,
			WeeksType: schedule.WeeksType,

			Members: schedule.Members.Select(m => new ScheduleMemberResponce(
				FirstName: m.User.FirstName,
				LastName: m.User.LastName,
				Role: m.Role
			)).ToList(),

			Lessons: schedule.Lessons.Select(l => new LessonResponce(
				Name: l.Name,
				Description: l.Description,
				Audience: l.Audience,
				Type: l.LessonType.Value,
				StartTime: l.StartTime,
				EndTime: l.EndTime,
				WeeksType: l.SheduleWeeksType,
				Day: l.DayOfWeek
			)).ToList()
		);
	}

	[HttpGet]
	public async Task<ActionResult<ScheduleByUserResponce[]>> GetAllByUser(CancellationToken cancellationToken)
	{
		var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

		if (!long.TryParse(userIdString, out long userId))
		{
			return BadRequest("Упс, ошибка! Необходимо перезайти в профиль!");
		}

		var specification = new SchedulesByUserSpec(userId, isTracking: false);
		var result = await _scheduleService.GetAllBySpecification(specification, cancellationToken);

		if (result.IsFailure)
		{
			return BadRequest(result.Error);
		}

		var schedules = result.Value;

		return schedules.Select(schedule => new ScheduleByUserResponce
		(
			Name: schedule.Name,
			Type: schedule.Type,
			Role: schedule.Members.First(x => x.UserId == userId).Role

		)).ToArray();
	}

	[HttpGet("All")]
	[Authorize(Roles = AppRoles.Administrator)]
	public async Task<ActionResult<ScheduleResponce[]>> GetAll(CancellationToken cancellationToken)
	{
		var specification = new AllSchedulesWithMembersAndLessonsSpec(isTracking: false, includeDeleted: true);
		var result = await _scheduleService.GetAllBySpecification(specification, cancellationToken);

		if (result.IsFailure)
		{
			return BadRequest(result.Error);
		}

		var schedules = result.Value;

		return schedules.Select(schedule => new ScheduleResponce
		(
			Name: schedule.Name,
			Description: schedule.Description,
			Type: schedule.Type,
			WeeksType: schedule.WeeksType,

			Members: schedule.Members.Select(m => new ScheduleMemberResponce(
				FirstName: m.User.FirstName,
				LastName: m.User.LastName,
			Role: m.Role
			)).ToList(),

			Lessons: schedule.Lessons.Select(l => new LessonResponce(
				Name: l.Name,
				Description: l.Description,
				Audience: l.Audience,
				Type: l.LessonType.Value,
				StartTime: l.StartTime,
				EndTime: l.EndTime,
				WeeksType: l.SheduleWeeksType,
				Day: l.DayOfWeek
			)).ToList()

		)).ToArray();
	}


	[HttpPost]
	public async Task<ActionResult<long>> Create(CreateScheduleRequest request, CancellationToken cancellationToken)
	{
		var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

		if (!long.TryParse(userIdString, out long userId))
		{
			return BadRequest("Упс, ошибка! Необходимо перезайти в профиль!");
		}

		var createDto = new CreateScheduleDto(request.Name, request.Description, request.Type, request.WeeksType, request.InstitusionId, userId);
		var result = await _scheduleService.CreateAsync(createDto, cancellationToken);

		if (result.IsFailure)
		{
			return BadRequest(result.Error);
		}

		var scheduleId = result.Value;
		return Ok(scheduleId);
	}

	public record ScheduleResponce(string Name,
		string? Description,
		ScheduleType Type,
		ScheduleWeeksType WeeksType,
		List<ScheduleMemberResponce> Members,
		List<LessonResponce> Lessons);

	public record ScheduleByUserResponce(string Name, ScheduleType Type, ScheduleRole Role);

	public record ScheduleMemberResponce(string FirstName,
		string LastName,
		ScheduleRole Role);

	public record LessonResponce(string Name,
		string? Description,
		string? Audience,
		string Type,
		TimeOnly StartTime,
		TimeOnly EndTime,
		ScheduleWeeksType WeeksType,
		DayOfWeek Day);


}
