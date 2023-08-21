using System.ComponentModel.DataAnnotations;

namespace Streaky.Udemy.DTOs;

public class EditAdminDTO
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}

