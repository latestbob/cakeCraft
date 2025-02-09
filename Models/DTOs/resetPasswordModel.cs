



namespace CakeCraftApi.Models.DTOs
{
    public class resetPasswordModel
{
    public required string Email { get; set; } // Use email instead of username if applicable
    public required string Password {get; set;}

    public required string Token {get; set;}
    
}
}