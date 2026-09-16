using JobApplicationTracker.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var applications = new List<JobApplication>
{
    new JobApplication
    {
        Id = 1,
        Company = "Example corp",
        Position = "Software Engineer",
        Status = "Applied",
        AppliedDate = DateOnly.FromDateTime(DateTime.Today)
    }
};

app.MapGet("/api/applications", () =>
{
    return applications;
});

app.MapGet("/api/applications/{id}", (int id) =>
{
    var application = applications.FirstOrDefault(a => a.Id == id);

    return application is not null
        ? Results.Ok(application)
        : Results.NotFound();
});

app.MapPost("/api/applications", (JobApplication request) =>
{
    int newId = applications.Count > 0 
    ? applications.Max(a => a.Id) + 1 
    : 1;

    request.Id = newId;

    applications.Add(request);
    return Results.Created($"/api/applications/{newId}", request);
});

app.MapDelete("/api/applications/{id}", (int id) =>
{
    var application = applications.Find(a => a.Id == id);

    if (application is not null)
    {
        applications.Remove(application);
        return Results.NoContent();
    }

    return Results.NotFound();
});

app.MapPut("/api/applications/{id}", (int id, JobApplication request) =>
{
    var application = applications.FirstOrDefault(a => a.Id == id);

    if (application is not null)
    {
        application.Company = request.Company;
        application.Position = request.Position;
        application.Status = request.Status;
        application.AppliedDate = request.AppliedDate;
        application.Notes = request.Notes;
        
        return Results.NoContent();
    }

    return Results.NotFound();
});

app.Run();
