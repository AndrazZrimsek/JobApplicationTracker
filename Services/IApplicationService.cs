using JobApplicationTracker.Models;

namespace JobApplicationTracker.Services;

public interface IApplicationService
{
    IEnumerable<JobApplication> GetAll();
    JobApplication? GetById(int id);
    JobApplication AddApplication (JobApplication application);
    bool DeleteApplication (int id);
    bool UpdateApplication(int id, JobApplication request);
}