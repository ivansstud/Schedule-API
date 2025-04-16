using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScheduleProject.API.Dtos.Request;
using ScheduleProject.API.Dtos.Responce.Schedule;
using ScheduleProject.API.Extensions.Mapping;
using ScheduleProject.Core.Abstractions.Services;
using ScheduleProject.Core.Dtos.Schedule;
using ScheduleProject.Core.Entities.Enums;
using ScheduleProject.Core.Specifications;
using System.Security.Claims;

namespace ScheduleProject.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public partial class SchedulesController : ControllerBase
{
	private readonly IScheduleService _scheduleService;

	public SchedulesController(IScheduleService scheduleService)
	{
		_scheduleService = scheduleService;
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<ScheduleInfoResponce>> GetById(long id, CancellationToken cancellationToken)
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

		return schedule.MapToInfoResponce();
	}

	[HttpGet]
	public async Task<ActionResult<SchedulePreviewResponce[]>> GetAllByUser(CancellationToken cancellationToken)
	{
		var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

		if (!long.TryParse(userIdString, out long userId))
		{
			return BadRequest("Упс, ошибка! Необходимо перезайти в профиль!");
		}

		var specification = new SchedulesByUserSpec(userId, isTracking: false);
		var result = await _scheduleService.GetListBySpecification(specification, cancellationToken);

		if (result.IsFailure)
		{
			return BadRequest(result.Error);
		}

		var schedules = result.Value;

		return schedules.Select(schedule => schedule.MapToPreviewResponse(userId)).ToArray();
	}

	[HttpGet("All")]
	[Authorize(Roles = AppRoles.Administrator)]
	public async Task<ActionResult<ScheduleFullInfoResponce[]>> GetAll(CancellationToken cancellationToken)
	{
		var specification = new AllSchedulesWithMembersAndLessonsSpec(isTracking: false, includeDeleted: true);
		var result = await _scheduleService.GetListBySpecification(specification, cancellationToken);

		if (result.IsFailure)
		{
			return BadRequest(result.Error);
		}

		var schedules = result.Value;

		return schedules.Select(schedule => schedule.MapToFullInfoResponse()).ToArray();
	}

	[HttpPost]
	public async Task<ActionResult<long>> Create(CreateScheduleRequest request, CancellationToken cancellationToken)
	{
		var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

		if (!long.TryParse(userIdString, out long userId))
		{
			return BadRequest("Упс, ошибка! Необходимо перезайти в профиль!");
		}

		var createDto = new CreateScheduleDto(request.Name, request.Description, request.Type, request.WeeksType, null, userId);
		var result = await _scheduleService.CreateAsync(createDto, cancellationToken);

		if (result.IsFailure)
		{
			return BadRequest(result.Error);
		}

		var scheduleId = result.Value;
		return Ok(scheduleId);
	}

	[HttpPost("Institusion")]
	[Authorize(Roles = AppRoles.InstitusionAdder)]
	public async Task<ActionResult<long>> CreateForInstitusion(CreateScheduleForInstitusionRequest request, CancellationToken cancellationToken)
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
}
