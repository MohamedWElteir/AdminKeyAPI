namespace FirstAPI.DTOs
{
    public partial class UserLoginDto
    {
         public required string Email { get; init; }
         public required string Password { get; init; }

    }
}