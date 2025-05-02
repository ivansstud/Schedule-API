using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ScheduleProject.Application.Requests.Auth;
using ScheduleProject.Core.Abstractions.Services;
using ScheduleProject.Core.Entities;
using ScheduleProject.Infrastructure.DAL.Services;

namespace ScheduleProject.Infrastructure.Handlers.Auth;

public class RefreshTokensHandler : IRequestHandler<RefreshTokensCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthService _authService;
    private readonly ILogger<RefreshTokensHandler> _logger;

    public RefreshTokensHandler(IUnitOfWork unitOfWork, IAuthService authService, ILogger<RefreshTokensHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _authService = authService;
        _logger = logger;
    }
    
    public async Task<Result> Handle(RefreshTokensCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var accessToken = _authService.GetCurrentAccessToken();
            var refreshToken = _authService.GetCurrentRefreshToken();

            if (!_authService.TryGetLoginFromExpiredToken(accessToken, out var login))
            {
                _authService.RemoveTokensFromClient();
                return Result.Failure<AuthToken>("Ошибка авторизации. Повторите вход.");
            }

            var foundUser = await _unitOfWork.Db
                .Set<AppUser>()
                .Include(x => x.AuthToken)
                .Include(x => x.Roles)
                .FirstOrDefaultAsync(x => x.Login == login, cancellationToken);
            
            if (foundUser?.AuthToken is null ||
                foundUser.AuthToken.RefreshToken != refreshToken ||
                foundUser.AuthToken.RefreshTokenExpiryDate < DateTime.UtcNow)
            {
				_authService.RemoveTokensFromClient();
				return Result.Failure<AuthToken>("Ошибка авторизации. Повторите вход.");
            }
            
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            _authService.UpdateAuthToken(foundUser);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            _authService.AddTokensToClient(foundUser.AuthToken.AccessToken, foundUser.AuthToken.RefreshToken);

            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError("{Exception}:", ex.Message + ex.InnerException?.Message);
            
            await _unitOfWork.RollbackAsync();
			_authService.RemoveTokensFromClient();

			return Result.Failure("Упс! Во время входа произошла ошибка. Повторите вход");
        }
    }
}