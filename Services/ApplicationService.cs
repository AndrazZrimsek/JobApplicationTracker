using JobApplicationTracker.Data;
using JobApplicationTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationTracker.Services;

public class ApplicationService : IApplicationService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ApplicationService> _logger;

    public ApplicationService(
        ApplicationDbContext context,
        ILogger<ApplicationService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<PagedResult<JobApplication>> GetAllAsync(ApplicationQueryDto queryDto)
    {
        var query = _context.Applications.AsQueryable();

        if (!string.IsNullOrWhiteSpace(queryDto.Status))
        {
            query = query.Where(a => a.Status == queryDto.Status);
        }

        if (!string.IsNullOrWhiteSpace(queryDto.Company))
        {
            query = query.Where(a => a.Company.Contains(queryDto.Company));
        }

        var totalCount = await query.CountAsync();

        query = query
            .OrderByDescending(a => a.AppliedDate)
            .Skip(queryDto.PageSize*(queryDto.Page-1))
            .Take(queryDto.PageSize);

        var items = await query.ToListAsync();

        return new PagedResult<JobApplication>
        {
            Items = items,
            TotalCount = totalCount
        };
    }

    public async Task<JobApplication?> GetByIdAsync(int id)
    {
        var application = await _context.Applications.FindAsync(id);

        if (application is null)
        {
            _logger.LogInformation(
                "Job application {ApplicationId} was not found",
                id);
        }

        return application;
    }

    public async Task<JobApplication> CreateApplicationAsync (JobApplication application)
    {
        _context.Applications.Add(application);

        await _context.SaveChangesAsync();

        _logger.LogInformation(
            "Created job application {ApplicationId} for {Company}",
            application.Id,
            application.Company);

        return application;
    }

    public async Task<bool> DeleteApplicationAsync (int id)
    {
        var application = await _context.Applications.FindAsync(id);

        if (application is not null)
        {
            _context.Applications.Remove(application);
            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Deleted job application {ApplicationId}",
                id);

            return true;
        }

        _logger.LogWarning(
            "Job application {ApplicationId} was not found",
            id);
        return false;
    }

    public async Task<bool> UpdateApplicationAsync(int id, JobApplication request)
    {
        var application = await _context.Applications.FindAsync(id);

        if (application is not null)
        {
            application.Company = request.Company;
            application.Position = request.Position;
            application.Status = request.Status;
            application.AppliedDate = request.AppliedDate;
            application.Notes = request.Notes;
            
            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Updated job application {ApplicationId}",
                id);

            return true;
        }

        _logger.LogWarning(
            "Job application {ApplicationId} was not found",
            id);

        return false;
    }
}