namespace ScheduleProject.Core.Abstractions.Services;

public interface IPasswordService
{
	string Hash(string password);
	bool Verify(string hashedPassword, string providedPassword);
}