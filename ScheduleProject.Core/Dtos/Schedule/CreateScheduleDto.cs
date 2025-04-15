using ScheduleProject.Core.Entities.Enums;

namespace ScheduleProject.Core.Dtos.Schedule;

public record CreateScheduleDto(string Name, string? Description, ScheduleType Type, ScheduleWeeksType WeeksType, long? InstitusionId, long UserId);

