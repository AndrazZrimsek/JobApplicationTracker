using JobApplicationTracker.Data;
using JobApplicationTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationTracker.Services;

public class ApplicationService : IApplicationService
{
    private readonly ApplicationDbContext _context;
    public ApplicationService(ApplicationDbContext context)
    {
        _context = context;
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
        return await _context.Applications.FindAsync(id);
    }

    public async Task<JobApplication> CreateApplicationAsync (JobApplication application)
    {
        _context.Applications.Add(application);

        await _context.SaveChangesAsync();

        return application;
    }

    public async Task<bool> DeleteApplicationAsync (int id)
    {
        var application = await _context.Applications.FindAsync(id);

        if (application is not null)
        {
            _context.Applications.Remove(application);
            await _context.SaveChangesAsync();
            return true;
        }

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
            return true;
        }

        return false;
    }
}