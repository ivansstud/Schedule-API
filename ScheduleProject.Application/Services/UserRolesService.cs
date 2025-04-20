using CSharpFunctionalExtensions;
using ScheduleProject.Core.Abstractions.DAL;
using ScheduleProject.Core.Abstractions.Services;
using ScheduleProject.Core.Entities;
using ScheduleProject.Core.Specifications.AppUser;
using ScheduleProject.Core.Specifications.UserRole;

namespace ScheduleProject.Application.Services;

public sealed class UserRolesService : IUserRolesService
{
	private readonly IRepository<UserRole> _rolesRepository;
	private readonly IRepository<AppUser> _userRepository;

	public UserRolesService(IRepository<UserRole> rolesRepository, IRepository<AppUser> userRepository)
	{
		_rolesRepository = rolesRepository;
		_userRepository = userRepository;
	}

	public async Task<List<UserRole>> GetAll(bool includeDeleted = false, CancellationToken cancellationToken = default)
	{
		var rolesSpecification = new UserRolesSpec(isTracking: false, includeDeleted: includeDeleted);
		var roles = await _rolesRepository.GetListBySpecificationAsync(rolesSpecification, cancellationToken);

		return roles;
	}

	public async Task<Result> GiveToUser(long userId, long roleId, CancellationToken cancellationToken = default)
	{
		var userSpecification = new UserByIdWithRolesSpec(userId, isTracking: true, includeDeleted: true);
		var user = await _userRepository.GetSingleBySpecificationAsync(userSpecification, cancellationToken);

		if (user is null)
		{
			return Result.Failure("Пользователя с таким Id не существует");
		}

		var roleSpecification = new UserRoleByIdSpec(roleId, isTracking: true);
		var role = await _rolesRepository.GetSingleBySpecificationAsync(roleSpecification, cancellationToken);

		if (role is null)
		{
			return Result.Failure("Роли с таким Id не существует");
		}

		user.AddRole(role);
		await _rolesRepository.CommitAsync(cancellationToken);

		return Result.Success();
	}

	public async Task<Result> TakeFromUser(long userId, long roleId, CancellationToken cancellationToken = default)
	{
		var userSpecification = new UserByIdWithRolesSpec(userId, isTracking: true, includeDeleted: true);
		var user = await _userRepository.GetSingleBySpecificationAsync(userSpecification, cancellationToken);

		if (user is null)
		{
			return Result.Failure("Пользователя с таким Id не существует");
		}

		var role = user.Roles.FirstOrDefault(x => x.Id == roleId);

		if (role is null)
		{
			return Result.Failure($"У этого пользователя нет роли с id: {roleId}");
		}

		user.RemoveRole(role);
		await _userRepository.CommitAsync(cancellationToken);

		return Result.Success();
	}
}
