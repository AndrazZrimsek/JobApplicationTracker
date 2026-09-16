using System.ComponentModel.DataAnnotations;

public class UpdateJobApplicationDto
{
    [Required]
    [MaxLength(100)]
    public string Company { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Position { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = string.Empty;

    public DateOnly AppliedDate { get; set; }

    [MaxLength(1000)]
    public string? Notes { get; set; }
}