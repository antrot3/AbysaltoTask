using System.ComponentModel.DataAnnotations;

namespace AplicationLayer.DTOs
{
    public record RegisterDto(

        [Required]
        [EmailAddress]
        string Email,

        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        string Password,

        string? FullName
    );
}