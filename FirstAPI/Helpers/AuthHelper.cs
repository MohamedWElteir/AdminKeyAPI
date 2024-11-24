using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Dapper;
using FirstAPI.Data;
using FirstAPI.DTOs;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;


namespace FirstAPI.Helpers;

public class AuthHelper : ControllerBase
{
    private readonly DataContextDapper _datacontextDapper;

    public AuthHelper()
    {
        _datacontextDapper = DataContextDapper.GetInstance();
    }

    public byte[] GetPasswordHash(string password, byte[] passwordSalt)
    {
        var passwordHashString =
            $"{Environment.GetEnvironmentVariable("PASSWORD_KEY")}{Convert.ToBase64String(passwordSalt)}";
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
                Environment.GetEnvironmentVariable("TOKEN_KEY") ?? throw new Exception("Token key is null")
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

    public bool SetPassword(UserLoginDto user)
    {
        var passwordSalt = new byte[128 / 8];  // 128 bits salting logic
        using var generator = RandomNumberGenerator.Create();
        generator.GetNonZeroBytes(passwordSalt);

        var passwordHash = GetPasswordHash(user.Password, passwordSalt);
        const string storedProcedure = "TutorialAppSchema.spRegistration_Upsert";
        var sqlParameters = new DynamicParameters();

        // var passwordSaltParam = new SqlParameter("@PasswordSaltParam", SqlDbType.VarBinary) { Value = passwordSalt };
        // var passwordHashParam = new SqlParameter("@PasswordHashParam", SqlDbType.VarBinary) { Value = passwordHash };
        // var emailParam = new SqlParameter("@EmailParam", SqlDbType.VarChar) { Value = user.Email };

        sqlParameters.Add("@Email", user.Email);
        sqlParameters.Add("@PasswordSalt", passwordSalt);
        sqlParameters.Add("@PasswordHash", passwordHash);
        return _datacontextDapper.ExecuteSqlWithParameters(storedProcedure, sqlParameters, CommandType.StoredProcedure);
    }
}