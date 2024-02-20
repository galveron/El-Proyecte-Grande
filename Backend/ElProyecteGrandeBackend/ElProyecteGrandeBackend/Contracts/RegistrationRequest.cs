using System.ComponentModel.DataAnnotations;

namespace ElProyecteGrandeBackend.Services;

public record RegistrationRequest(
    [Required]string Email, 
    [Required]string Username, 
    [Required]string Password);