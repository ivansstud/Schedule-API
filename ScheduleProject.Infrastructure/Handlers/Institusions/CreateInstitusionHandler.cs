using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.Logging;
using ScheduleProject.Application.Requests.Institusions;
using ScheduleProject.Core.Entities;
using ScheduleProject.Infrastructure.Auth.Extensions;
using ScheduleProject.Infrastructure.DAL.Services;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

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

			var isUserExists = await _unitOfWork.Db
				.Set<AppUser>()
				.Where(x => x.Id == userId && x.IsDeleted == false)
				.AnyAsync(cancellationToken);

			if (isUserExists == false)
			{
				//TODO: Добавить крит лог
				return Result.Failure("Упс! Не удалось добавить учреждение. Перезайдите в профиль.");
			}

			var newInstitusionResult = Institution.Create(request.Name, request.ShortName, request.Description, userId);
			
			if (!newInstitusionResult.TryGetValue(out Institution? newInstitusion))
			{
				return Result.Failure(newInstitusionResult.Error);
			}

			await _unitOfWork.BeginTransactionAsync(cancellationToken);

			await _unitOfWork.Db.AddAsync(newInstitusion,cancellationToken);
			
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
