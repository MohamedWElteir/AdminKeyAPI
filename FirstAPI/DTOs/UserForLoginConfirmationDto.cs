namespace FirstAPI.DTOs
{
    public class UserForLoginConfirmationDto
    {
       public byte[] PasswordHash { get; set; } = Array.Empty<byte>(); // Array.Empty<byte>() is a new feature in C# 7.3
       public byte[] PasswordSalt { get; set; } = Array.Empty<byte>(); // Resolves rule CA1825 violation.
    }
}