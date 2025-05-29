using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ScheduleProject.Application.Requests.Auth;
using ScheduleProject.Core.Abstractions.Services;
using ScheduleProject.Core.Entities;
using ScheduleProject.Infrastructure.DAL.Services;

namespace ScheduleProject.Infrastructure.Handlers.Auth;

public class UserLoginHandler : IRequestHandler<UserLoginCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordService _passwordService;
    private readonly IAuthService _authService;
    private readonly ILogger<UserLoginHandler> _logger;
    
    public UserLoginHandler(IAuthService authService, ILogger<UserLoginHandler> logger, IUnitOfWork unitOfWork, IPasswordService passwordService)
    {
        _authService = authService;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _passwordService = passwordService;
    }

    public async Task<Result> Handle(UserLoginCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var foundUser = await _unitOfWork.Db
                .Set<AppUser>()
                .Include(x => x.AuthToken)
                .Include(x => x.Roles)
                .FirstOrDefaultAsync(x => x.Login == request.Login, cancellationToken);

            if (foundUser is null)
            {
                return Result.Failure<AuthToken>("Неверно введены логин или пароль");
            }

            var isVerifyPassword = _passwordService.Verify(foundUser.HashedPassword, request.Password);

            if (isVerifyPassword == false)
            {
                return Result.Failure<AuthToken>("Неверно введены логин или пароль");
            }
            
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            if (foundUser.AuthToken is not null)
            {
                _authService.UpdateAuthToken(foundUser);
            }
            else
            {
                var roles = foundUser.Roles.Select(x => x.Name).ToList();
                var authToken = _authService.CreateAuthToken(foundUser.Id, foundUser.Login, foundUser.FirstName, roles);

                foundUser.SetAuthToken(authToken);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            _authService.AddTokensToClient(foundUser.AuthToken!.AccessToken, foundUser.AuthToken.RefreshToken);

            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError("{Exception}", ex.Message + ex.InnerException?.Message);
            
            await _unitOfWork.RollbackAsync();
            
            return Result.Failure<AuthToken>("Упс! Во время входа произошла ошибка.");
        }
    }
}