using JobApplicationTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationTracker.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<JobApplication> Applications {get; set;}
    public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options) : base(options)
    {        
    }

}

