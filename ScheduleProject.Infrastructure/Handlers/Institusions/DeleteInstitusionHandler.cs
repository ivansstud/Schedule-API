using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ScheduleProject.Application.Requests.Institusions;
using ScheduleProject.Core.Entities;
using ScheduleProject.Infrastructure.Auth.Extensions;
using ScheduleProject.Infrastructure.DAL.Services;
using System.Security.Claims;

namespace ScheduleProject.Infrastructure.Handlers.Institusions;

class DeleteInstitusionHandler : IRequestHandler<DeleteInstitusionCommand, Result>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ILogger<DeleteInstitusionHandler> _logger;
	private readonly ClaimsPrincipal _user;

	public DeleteInstitusionHandler(IUnitOfWork unitOfWork, ILogger<DeleteInstitusionHandler> logger, ClaimsPrincipal user)
	{
		_unitOfWork = unitOfWork;
		_logger = logger;
		_user = user;
	}

	public async Task<Result> Handle(DeleteInstitusionCommand request, CancellationToken cancellationToken)
	{
		try
		{
			var userId = _user.GetId();

			var institusionForDelete = await _unitOfWork.Db
				.Set<Institution>()
				.FirstOrDefaultAsync(cancellationToken);

			if (institusionForDelete == null)
			{
				return Result.Failure("Удаляемое учреждение не существует.");
			}

			bool userIsInstitusionOwner = institusionForDelete.OwnerId == userId;

			if (!userIsInstitusionOwner && !_user.IsAdmin())
			{
				return Result.Failure("У вас нет прав для удаления этого учреждения.");
			}

			await _unitOfWork.BeginTransactionAsync(cancellationToken);

			institusionForDelete.SetDeleted(true);

			await _unitOfWork.SaveChangesAsync(cancellationToken);
			await _unitOfWork.CommitAsync(cancellationToken);

			return Result.Success();
		}
		catch (Exception ex)
		{
			_logger.LogError("{Exception}:", ex.Message + ex.InnerException?.Message);

			await _unitOfWork.RollbackAsync();

			return Result.Failure("Упс! Не удалось удалить учреждение.");
		}
	}
}
