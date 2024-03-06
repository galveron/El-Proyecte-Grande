using System.ComponentModel.DataAnnotations;

namespace ElProyecteGrandeBackend.Services;

public record RegistrationRequestCompany(
    [Required]string Email, 
    [Required]string Username, 
    [Required]string Password,
    [Required]string CompanyName,
    [Required]string Identifier);