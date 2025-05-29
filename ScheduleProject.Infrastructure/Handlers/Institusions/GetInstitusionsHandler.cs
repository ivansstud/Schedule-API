using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ScheduleProject.Application.DTOs.Institusions;
using ScheduleProject.Application.Requests.Institusions;
using ScheduleProject.Core.Entities;
using ScheduleProject.Infrastructure.DAL.Services;
using ScheduleProject.Infrastructure.Extensions.Mapping;

namespace ScheduleProject.Infrastructure.Handlers.Institusions;

public class GetInstitusionsHandler : IRequestHandler<GetInstituisonsRequest, Result<InstitusionShortInfoDto[]>>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ILogger<GetInstitusionsHandler> _logger;

	public GetInstitusionsHandler(IUnitOfWork unitOfWork, ILogger<GetInstitusionsHandler> logger)
	{
		_unitOfWork = unitOfWork;
		_logger = logger;
	}

	public async Task<Result<InstitusionShortInfoDto[]>> Handle(GetInstituisonsRequest request, CancellationToken cancellationToken)
	{
		try
		{
			var substring = request.NameSubstring;

			if (substring.Length < Institution.MinShortNameLength)
			{
				return Result.Failure<InstitusionShortInfoDto[]>($"Для получения учреждений необходимо ввести минимум {Institution.MinShortNameLength} символов.");
			}

			var institusions = await _unitOfWork.Db
				.Set<Institution>()
				.Where(x => x.IsDeleted == false
					&& EF.Functions.Like(x.Name, $"%{substring}%") || EF.Functions.Like(x.ShortName, $"%{substring}%"))
				.Select(x => x.MapToShortInfo())
				.Take(request.Take)
				.Skip(request.Skip)
				.ToArrayAsync(cancellationToken);

			return institusions;
		}
		catch (Exception ex)
		{
			_logger.LogError("{Exception}:", ex.Message + ex.InnerException?.Message);

			return Result.Failure<InstitusionShortInfoDto[]>("Упс! Не удалось получить учреждения.");
		}
	}
}
