using CSharpFunctionalExtensions;
using ScheduleProject.Core.Entities;

namespace ScheduleProject.Core.Abstractions.Services;

public interface IUserRolesService
{
	Task<Result> GiveToUser(long userId, long roleId, CancellationToken cancellationToken = default);
	
	Task<Result> TakeFromUser(long userId, long roleId, CancellationToken cancellationToken = default);

	Task<List<UserRole>> GetAll(bool includeDeleted = false, CancellationToken cancellationToken = default);
}