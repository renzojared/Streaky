using System.ComponentModel.DataAnnotations;

namespace Streaky.Udemy.DTOs;

public class CredentialUser
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}

