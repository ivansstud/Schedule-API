using CSharpFunctionalExtensions;
using MediatR;
using ScheduleProject.Core.Entities;

namespace ScheduleProject.Application.Requests.Auth;

public class UserLoginCommand : IRequest<Result>
{
    public UserLoginCommand(string login, string password)
    {
        Login = login;
        Password = password;
    }

    public string Login { get; }
    public string Password { get; }
}