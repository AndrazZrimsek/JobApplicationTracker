using JobApplicationTracker.Models;
using JobApplicationTracker.Services;
using Microsoft.AspNetCore.Mvc;

namespace JobApplicationTracker.Controllers;

[ApiController]
[Route("api/applications")]
public class ApplicationsController : ControllerBase
{
    private readonly IApplicationService _applicationService;
    public ApplicationsController(IApplicationService applicationService)
    {
        _applicationService = applicationService;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_applicationService.GetAll());
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var application = _applicationService.GetById(id);

        return application is not null
            ? Ok(application)
            : NotFound();
    }

    [HttpPost]
    public IActionResult AddApplication (JobApplication application)
    {
        var newApplication = _applicationService.AddApplication(application);
        return Created($"/api/applications/{newApplication.Id}", newApplication);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteApplication (int id)
    {
        return _applicationService.DeleteApplication(id)
            ? NoContent()
            : NotFound();
    }

    [HttpPut("{id}")]
    public IActionResult UpdateApplication(int id, JobApplication request)
    {
        return _applicationService.UpdateApplication(id, request)
            ? NoContent()
            : NotFound();
    }
}