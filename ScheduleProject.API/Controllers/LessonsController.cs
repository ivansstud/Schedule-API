using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using ScheduleProject.API.Dtos.Responce.Lesson;
using ScheduleProject.API.Extensions.Mapping;
using ScheduleProject.Core.Abstractions.Services;
using ScheduleProject.Core.Dtos.Lesson;
using ScheduleProject.Core.Specifications.Lesson;
using System.Reflection.Metadata.Ecma335;

namespace ScheduleProject.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LessonsController : ControllerBase
{
	private readonly ILessonsService _lessonsService;

	public LessonsController(ILessonsService lessonsService)
	{
		_lessonsService = lessonsService;
	}

	[HttpPost]
	public async Task<ActionResult<long>> Create(CreateLessonDto createDto, CancellationToken cancellationToken)
	{
		var result = await _lessonsService.CreateAsync(createDto, cancellationToken);

		if (result.IsFailure)
		{
			return BadRequest(result.Error);
		}

		return Ok(result.Value);
	}

	[HttpGet("Schedules/{scheduleId}")]
	public async Task<ActionResult<List<LessonInfoResponce>>> GetAllBySchedule(long scheduleId, CancellationToken cancellationToken)
	{
		var specification = new LessonByScheduleSpec(scheduleId, false);
		var lessonsResult = await _lessonsService.GetListBySpecificationAsync(specification, cancellationToken);

		if (lessonsResult.IsFailure)
		{
			return BadRequest(lessonsResult.Error);
		}

		return lessonsResult.Value.Select(lesson => lesson.MapToInfoResponce()).ToList();
	}

	[HttpDelete]
	public async Task<IActionResult> Delete([FromBody]long lessonId, CancellationToken cancellationToken)
	{
		var result = await _lessonsService.DeleteAsync(lessonId, cancellationToken);

		if (result.IsFailure)
		{
			return BadRequest(result.Error);
		}

		return Ok();
	}
}
