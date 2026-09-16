using JobApplicationTracker.Models;

namespace JobApplicationTracker.Services;

public class ApplicationService : IApplicationService
{
    private readonly List<JobApplication> _applications;
    public ApplicationService()
    {
        _applications = new List<JobApplication>();
    }

    public IEnumerable<JobApplication> GetAll()
    {
        return _applications;
    }

    public JobApplication? GetById(int id)
    {
        var application = _applications.FirstOrDefault(a => a.Id == id);
        return application;
    }

    public JobApplication AddApplication (JobApplication application)
    {
        int newId = _applications.Count > 0
            ? _applications.Max(a => a.Id) + 1
            : 1;

        application.Id = newId;

        _applications.Add(application);
        return application;
    }

    public bool DeleteApplication (int id)
    {
        var application = _applications.Find(a => a.Id == id);

        if (application is not null)
        {
            _applications.Remove(application);
            return true;
        }

        return false;
    }

    public bool UpdateApplication(int id, JobApplication request)
    {
        var application = _applications.Find(a => a.Id == id);
        if (application is not null)
        {
            application.Company = request.Company;
            application.Position = request.Position;
            application.Status = request.Status;
            application.AppliedDate = request.AppliedDate;
            application.Notes = request.Notes;
            
            return true;
        }

        return false;
    }
}