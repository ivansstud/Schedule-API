using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScheduleProject.Core.Entities;
using ScheduleProject.Core.Entities.Enums;

namespace ScheduleProject.Core.Tests.Entities;

[TestClass]
public class AppUserTests
{
    private const string ValidLogin = "testuser";
    private const string ValidFirstName = "Test";
    private const string ValidLastName = "User";
    private const string ValidPasswordHash = "hashedPassword123";

    [TestMethod]
    public void Create_WithTooShortLogin_ShouldReturnFailure()
    {
        // Arrange
        var login = new string('A', AppUser.MinLoginLength -1); 

        // Act
        var result = AppUser.Create(login, ValidPasswordHash, ValidFirstName, ValidLastName);

        // Assert
        Assert.IsTrue(result.IsFailure);
    }

    [TestMethod]
    public void Create_WithTooLongFirstName_ShouldReturnFailure()
    {
        // Arrange
        var login = ValidLogin;
        var firstName = new string('A', AppUser.MaxFirstNameLength + 1);
        var lastName = ValidLastName;
        var passwordHash = ValidPasswordHash;

        // Act
        var result = AppUser.Create(login, passwordHash, firstName, lastName);

        // Assert
        Assert.IsTrue(result.IsFailure);
    }
}