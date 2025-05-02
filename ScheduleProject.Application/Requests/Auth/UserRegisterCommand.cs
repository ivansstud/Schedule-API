using CSharpFunctionalExtensions;
using MediatR;

namespace ScheduleProject.Application.Requests.Auth;

public sealed class UserRegisterCommand : IRequest<Result>
{
    public UserRegisterCommand(string login, string password, string firstName, string lastName)
    {
        Login = login;
        Password = password;
        FirstName = firstName;
        LastName = lastName;
    }
    
    public string Login { get; }
    public string Password { get; }
    public string FirstName { get; }
    public string LastName { get; }
}