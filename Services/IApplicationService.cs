using JobApplicationTracker.Models;

namespace JobApplicationTracker.Services;

public interface IApplicationService
{
    Task<IEnumerable<JobApplication>> GetAllAsync();
    Task<JobApplication?> GetByIdAsync(int id);
    Task<JobApplication> CreateApplicationAsync(JobApplication application);
    Task<bool> DeleteApplicationAsync(int id);
    Task<bool> UpdateApplicationAsync(int id, JobApplication request);
}