using System.ComponentModel.DataAnnotations;

public class ApplicationQueryDto
{
    public string? Status { get; set; }

    public string? Company { get; set; }

    [Range(1, int.MaxValue)]
    public int Page { get; set; } = 1;

    [Range(1, 100)]
    public int PageSize { get; set; } = 20;
}