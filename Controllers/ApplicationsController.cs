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
    public async Task<IActionResult> GetAll([FromQuery] ApplicationQueryDto query)
    {
        var result = await _applicationService.GetAllAsync(query);
        return Ok(result);
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
    public async Task<IActionResult> AddApplication (CreateJobApplicationDto request)
    {
        var application = new JobApplication
        {
            Company = request.Company,
            Position = request.Position,
            Status = request.Status,
            AppliedDate = request.AppliedDate,
            Notes = request.Notes
        };

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
    public async Task<IActionResult> UpdateApplication(int id, UpdateJobApplicationDto request)
    {
        var application = new JobApplication
        {
            Company = request.Company,
            Position = request.Position,
            Status = request.Status,
            AppliedDate = request.AppliedDate,
            Notes = request.Notes
        };

        return await _applicationService.UpdateApplicationAsync(id, application)
            ? NoContent()
            : NotFound();
    }
}