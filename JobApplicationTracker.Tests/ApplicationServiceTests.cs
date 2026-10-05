using JobApplicationTracker.Models;
using JobApplicationTracker.Services;
using JobApplicationTracker.Tests.Helpers;
using Microsoft.Extensions.Logging.Abstractions;

namespace JobApplicationTracker.Tests;

public class ApplicationServiceTests
{
    [Fact]
    public async Task CreateApplicationAsync_SavesApplicationAndGeneratesId()
    {
        using var database = new TestDatabase();

        var service = new ApplicationService(database.Context, NullLogger<ApplicationService>.Instance);

        // Create a test application entry
        var application = new JobApplication
        {
            Company = "Test Corp",
            Position = "Software Engineer",
            Status = "Applied",
            AppliedDate = DateOnly.FromDateTime(DateTime.Today),
            Notes = "Created from unit test"
        };

        // Create the application in the app
        var created = await service.CreateApplicationAsync(application);

        Assert.True(created.Id > 0);

        // Clear EF tracking to ensure the application is read from the database
        database.Context.ChangeTracker.Clear();

        var savedApplication = await database.Context.Applications.FindAsync(created.Id);

        Assert.NotNull(savedApplication);
        Assert.Equal("Test Corp", savedApplication.Company);
    }

    [Fact]
    public async Task GetByIdAsync_CallReturnsValidApplication()
    {
        using var database = new TestDatabase();

        var service = new ApplicationService(database.Context, NullLogger<ApplicationService>.Instance);

        var application = new JobApplication
        {
            Company = "Test Corp",
            Position = "Software Engineer",
            Status = "Applied",
            AppliedDate = DateOnly.FromDateTime(DateTime.Today),
            Notes = "Created from unit test"
        };

        database.Context.Applications.Add(application);
        await database.Context.SaveChangesAsync();

        // Clear EF tracking to ensure the application is read from the database
        database.Context.ChangeTracker.Clear();

        var result = await service.GetByIdAsync(application.Id);

        Assert.NotNull(result);
        Assert.Equal(application.Id, result.Id);
        Assert.Equal(application.Company, result.Company);
        Assert.Equal(application.Position, result.Position);
        Assert.Equal(application.Status, result.Status);
        Assert.Equal(application.AppliedDate, result.AppliedDate);
        Assert.Equal(application.Notes, result.Notes);
    }

    [Fact]
    public async Task GetByIdAsync_MissingId_ReturnsNull()
    {
        using var database = new TestDatabase();

        var service = new ApplicationService(database.Context, NullLogger<ApplicationService>.Instance);

        var result = await service.GetByIdAsync(999);

        Assert.Null(result);
    }
    
    [Fact]
    public async Task UpdateApplicationAsync_ExistingApplication_UpdatesAndReturnsTrue()
    {
        using var database = new TestDatabase();

        var service = new ApplicationService(database.Context, NullLogger<ApplicationService>.Instance);

        var todayDate = DateOnly.FromDateTime(DateTime.Today);
        var application = new JobApplication
        {
            Company = "Test Corp",
            Position = "Software Engineer",
            Status = "Applied",
            AppliedDate = todayDate,
            Notes = "Created from unit test"
        };

        database.Context.Applications.Add(application);
        await database.Context.SaveChangesAsync();

        var updatedApplication = new JobApplication
        {
            Company = "New Test Corp",
            Position = "Software Engineer II",
            Status = "Interview",
            AppliedDate = todayDate,
            Notes = "Value updated"
        };

        var response = await service.UpdateApplicationAsync(application.Id, updatedApplication);
        Assert.True(response);

        // Clear EF tracking to ensure the application is read from the database
        database.Context.ChangeTracker.Clear();

        var result = await service.GetByIdAsync(application.Id);

        Assert.NotNull(result);
        Assert.Equal(application.Id, result.Id);
        Assert.Equal(updatedApplication.Company, result.Company);
        Assert.Equal(updatedApplication.Position, result.Position);
        Assert.Equal(updatedApplication.Status, result.Status);
        Assert.Equal(updatedApplication.AppliedDate, result.AppliedDate);
        Assert.Equal(updatedApplication.Notes, result.Notes);
    }

    [Fact]
    public async Task UpdateApplicationAsync_MissingId_ReturnsFalse()
    {
        using var database = new TestDatabase();

        var service = new ApplicationService(database.Context, NullLogger<ApplicationService>.Instance);

        var todayDate = DateOnly.FromDateTime(DateTime.Today);
        var application = new JobApplication
        {
            Company = "Test Corp",
            Position = "Software Engineer",
            Status = "Applied",
            AppliedDate = todayDate
        };

        var response = await service.UpdateApplicationAsync(999, application);
        Assert.False(response);
    }
}