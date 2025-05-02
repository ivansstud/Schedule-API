using MediatR;
using ScheduleProject.Application.Requests.Lessons;

namespace ScheduleProject.API.Endpoints;

public static class LessonsEndpoints
    {
        public static void MapLessonsEndpoints(this WebApplication app)
        {
            var authGroup = app.MapGroup("api/lessons")
                .RequireAuthorization();

            authGroup.MapPost("", CreateHandler);
            
            authGroup.MapGet("schedule/{scheduleId:long}", AllByScheduleHandler);
            
            authGroup.MapDelete("", DeleteHandler);
        }
        
        private static async Task<IResult> CreateHandler(
            CreateLessonCommand command,
            IMediator mediator,
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(command, cancellationToken);
            
            if (result.IsFailure)
            {
                return Results.BadRequest(result.Error);
            }

            return Results.Created();

        }
        
        private static async Task<IResult> AllByScheduleHandler(
            long scheduleId,
            IMediator mediator,
            CancellationToken cancellationToken)
        {
            var request = new LessonsByScheduleRequest(scheduleId);
            var result = await mediator.Send(request, cancellationToken);
            
            if (result.IsFailure)
            {
                return Results.BadRequest(result.Error);
            }

            return Results.Ok(result.Value);
        }
        
        private static async Task<IResult> DeleteHandler(
            long id,
            IMediator mediator,
            CancellationToken cancellationToken)
        {
            var request = new DeleteLessonCommand(id);
            var result = await mediator.Send(request, cancellationToken);
            
            if (result.IsFailure)
            {
                return Results.BadRequest(result.Error);
            }

            return Results.NoContent();
        }
    }