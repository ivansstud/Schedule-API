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

class UpdateInstitusionHandler : IRequestHandler<UpdateInstitusionCommand, Result>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ILogger<UpdateInstitusionHandler> _logger;
	private readonly ClaimsPrincipal _user;

	public UpdateInstitusionHandler(IUnitOfWork unitOfWork, ILogger<UpdateInstitusionHandler> logger, ClaimsPrincipal user)
	{
		_unitOfWork = unitOfWork;
		_logger = logger;
		_user = user;
	}

	public async Task<Result> Handle(UpdateInstitusionCommand request, CancellationToken cancellationToken)
	{
		try
		{
			var userId = _user.GetId();

			var institusionForUpdate = await _unitOfWork.Db
				.Set<Institution>()
				.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

			if (institusionForUpdate == null)
			{
				return Result.Failure("Изменяемого учреждения не существует.");
			}

			bool userIsInstitusionOwner = institusionForUpdate.OwnerId == userId;

			if (!userIsInstitusionOwner && !_user.IsAdmin())
			{
				return Result.Failure("У вас нет прав для изменения данных этого учреждения.");
			}

			await _unitOfWork.BeginTransactionAsync(cancellationToken);

			var results = new Result[]
			{
				institusionForUpdate.SetName(request.Name),
				institusionForUpdate.SetShortName(request.ShortName),
				institusionForUpdate.SetDescription(request.Description)
			};

			var errors = results
				.Where(x => x.IsFailure)
				.Select(x => x.Error)
				.ToList();

			if (errors.Count != 0)
			{
				await _unitOfWork.RollbackAsync();
				return Result.Failure(string.Join(" \n", errors));
			}

			await _unitOfWork.SaveChangesAsync(cancellationToken);
			await _unitOfWork.CommitAsync(cancellationToken);

			return Result.Success();
		}
		catch (Exception ex)
		{
			_logger.LogError("{Exception}:", ex.Message + ex.InnerException?.Message);

			await _unitOfWork.RollbackAsync();

			return Result.Failure("Упс! Не удалось обновить информацию об учреждении.");
		}
	}
}
