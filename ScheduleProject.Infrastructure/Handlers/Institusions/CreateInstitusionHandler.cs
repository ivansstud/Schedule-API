using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.Logging;
using ScheduleProject.Application.Requests.Institusions;
using ScheduleProject.Core.Entities;
using ScheduleProject.Infrastructure.Auth.Extensions;
using ScheduleProject.Infrastructure.DAL.Services;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using ScheduleProject.Core.Entities.Enums;

namespace ScheduleProject.Infrastructure.Handlers.Institusions;

public class CreateInstitusionHandler : IRequestHandler<CreateInstitusionCammand, Result>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ILogger<CreateInstitusionHandler> _logger;
	private readonly ClaimsPrincipal _user;

	public CreateInstitusionHandler(IUnitOfWork unitOfWork, ILogger<CreateInstitusionHandler> logger, ClaimsPrincipal user)
	{
		_unitOfWork = unitOfWork;
		_logger = logger;
		_user = user;
	}

	public async Task<Result> Handle(CreateInstitusionCammand request, CancellationToken cancellationToken)
	{
		try
		{
			var userId = _user.GetId();

			var newInstitusionResult = Institution.Create(request.Name, request.ShortName, request.Description, userId);
			
			if (!newInstitusionResult.TryGetValue(out Institution? newInstitusion))
			{
				return Result.Failure(newInstitusionResult.Error);
			}

			if (!_user.IsInstitusionUser() && !_user.IsAdmin())
			{
				return Result.Failure("У вас нет прав для создания нового учреждения.");
			}

			await _unitOfWork.BeginTransactionAsync(cancellationToken);

			var user = await _unitOfWork.Db
				.Set<AppUser>()
				.FirstOrDefaultAsync(u => u.Id == userId && !u.IsDeleted, cancellationToken);

			if (user is null)
			{
				return Result.Failure("Ошибка авторизации");
			}

			newInstitusion.SetOwner(user);

			await _unitOfWork.SaveChangesAsync(cancellationToken);
			await _unitOfWork.CommitAsync(cancellationToken);

			return Result.Success();
		}
		catch (Exception ex)
		{
			_logger.LogError("{Exception}:", ex.Message + ex.InnerException?.Message);

			await _unitOfWork.RollbackAsync();

			return Result.Failure("Упс! Не удалось добавить учреждение.");
		}
	}
}
