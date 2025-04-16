using ScheduleProject.Core.Entities.Enums;

namespace ScheduleProject.API.Dtos.Request;

public record CreateScheduleForInstitusionRequest(string Name, string? Description, ScheduleType Type, ScheduleWeeksType WeeksType, long InstitusionId);
