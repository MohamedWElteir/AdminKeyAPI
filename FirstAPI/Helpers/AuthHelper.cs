using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;

namespace FirstAPI.Helpers;

public class AuthHelper
{
    private readonly IConfiguration _config;
    public AuthHelper(IConfiguration configuration)
    {
        _config = configuration;
    }
    public byte[] GetPasswordHash(string password, byte[] passwordSalt)
    {
        var passwordHashString =
            $"{_config.GetSection("AppSettings:PasswordKey").Value}{Convert.ToBase64String(passwordSalt)}";
        var passwordHash = KeyDerivation.Pbkdf2(
            password: password,
            salt: Encoding.ASCII.GetBytes(passwordHashString),
            prf: KeyDerivationPrf.HMACSHA512,
            iterationCount: 10000,
            numBytesRequested: 256 / 8
        );
        return passwordHash;
    }

    public string CreateToken(int userId)
    {
        var claims = new[]
        {
            new Claim("userId", userId.ToString())
        };

        SymmetricSecurityKey key = new(
            Encoding.UTF8.GetBytes(
                _config.GetSection("AppSettings:TokenKey").Value ?? throw new Exception("TokenKey is null")
            )
        );
        SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha512Signature);
        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(claims),
            SigningCredentials = credentials,
            Expires = DateTime.Now.AddDays(1)
        };

        JwtSecurityTokenHandler tokenHandler = new();
        SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(securityToken);
    }
}