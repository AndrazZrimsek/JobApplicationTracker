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
    public async Task<IActionResult> GetAll()
    {
        var applications = await _applicationService.GetAllAsync();
        return Ok(applications);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var application = await _applicationService.GetByIdAsync(id);

        return application is not null
            ? Ok(application)
            : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> AddApplication (JobApplication application)
    {
        var newApplication = await _applicationService.CreateApplicationAsync(application);
        return Created($"/api/applications/{newApplication.Id}", newApplication);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteApplication (int id)
    {
        return await _applicationService.DeleteApplicationAsync(id)
            ? NoContent()
            : NotFound();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateApplication(int id, JobApplication request)
    {
        return await _applicationService.UpdateApplicationAsync(id, request)
            ? NoContent()
            : NotFound();
    }
}