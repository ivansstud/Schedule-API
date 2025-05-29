using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ScheduleProject.Application.Requests.Auth;
using ScheduleProject.Core.Abstractions.Services;
using ScheduleProject.Core.Entities;
using ScheduleProject.Core.Entities.Enums;
using ScheduleProject.Infrastructure.DAL.Services;

namespace ScheduleProject.Infrastructure.Handlers.Auth;

public class UserRegisterHandler : IRequestHandler<UserRegisterCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordService _passwordService;
    private readonly ILogger<UserRegisterHandler> _logger;

    public UserRegisterHandler(IUnitOfWork unitOfWork, ILogger<UserRegisterHandler> logger, IPasswordService passwordService)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _passwordService = passwordService;
    }
    
    public async Task<Result> Handle(UserRegisterCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userAlreadyExists = await _unitOfWork.Db
                .Set<AppUser>()
                .AnyAsync(x => x.Login == request.Login, cancellationToken);
            
            if (userAlreadyExists)
            {
                return Result.Failure("Пользоваетль с таким логиным уже существует.");
            }

            var hashedPassword = _passwordService.Hash(request.Password);
            var newUserResult = AppUser.Create(request.Login, hashedPassword, request.FirstName, request.LastName);
            
            if (newUserResult.IsFailure)
            {
                return Result.Failure(newUserResult.Error);
            }

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            var defaultRole = await _unitOfWork.Db
                .Set<UserRole>()
                .FirstAsync(x => x.Name == AppRoles.DomainUser, cancellationToken);

            var newUser = newUserResult.Value;
            newUser.AddRole(defaultRole);

            await _unitOfWork.Db.AddAsync(newUser, cancellationToken);
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError("{Exception}", ex.Message + ex.InnerException?.Message);
            
            await _unitOfWork.RollbackAsync();
            
            return Result.Failure("Упс! Во время регистрации произошла ошибка.");
        }
    }
}