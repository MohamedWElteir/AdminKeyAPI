using System.Data;
using System.Security.Cryptography;
using FirstAPI.Data;
using FirstAPI.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using FirstAPI.Helpers;

namespace FirstAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly DataContextDapper _dapper;
        private readonly AuthHelper _authHelper;
        public AuthController(IConfiguration config)
        {

            _dapper = new DataContextDapper(config);
            _authHelper = new AuthHelper(config);
        }


        [AllowAnonymous]
        [HttpPost("Register")]
        public IActionResult Register(UserRegistrationDto user)
        {
            if (user.Password != user.ConfirmPassword)
            {
                return BadRequest("Passwords do not match");
            }

            var exisingUsersEnumerable =
                _dapper.LoadData<string>($"SELECT Email FROM TutorialAppSchema.Auth WHERE Email = '{user.Email}'");

            if (exisingUsersEnumerable.Any()) return Conflict("User already exists");


            var passwordSalt = new byte[128 / 8];
            using var generator = RandomNumberGenerator.Create();
            generator.GetNonZeroBytes(passwordSalt);

            var passwordHash = _authHelper.GetPasswordHash(user.Password, passwordSalt);
            var sql = $"INSERT INTO TutorialAppSchema.Auth (Email, PasswordHash, PasswordSalt) VALUES ('{user.Email}', @PasswordHash, @PasswordSalt)";
            List<SqlParameter> parameters = new List<SqlParameter>();
            var passwordSaltParam = new SqlParameter("@PasswordSalt", SqlDbType.VarBinary) { Value = passwordSalt };
            var passwordHashParam = new SqlParameter("@PasswordHash", SqlDbType.VarBinary) { Value = passwordHash };
            parameters.Add(passwordSaltParam);
            parameters.Add(passwordHashParam);
            if (!_dapper.ExecuteSqlWithParameters(sql, parameters)) return StatusCode(500);
            var sqlAddUser = $"INSERT INTO TutorialAppSchema.Users (Email, FirstName, LastName, Gender, Active) VALUES ('{user.Email}', '{user.FirstName}', '{user.LastName}', '{user.Gender}',0)";
            return _dapper.ExecuteSql(sqlAddUser) ? Ok("User Registered!") : StatusCode(500, "User Registration Failed");
        }
        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login(UserLoginDto user)
        {
            var sql = $"SELECT [PasswordHash], [PasswordSalt] FROM TutorialAppSchema.Auth WHERE Email = '{user.Email}'";
            try
            {
                var dbUser = _dapper.LoadDataSingle<UserForLoginConfirmationDto>(sql);
                var passwordHash = _authHelper.GetPasswordHash(user.Password, dbUser.PasswordSalt);
                if(!passwordHash.SequenceEqual(dbUser.PasswordHash))
                {
                   return Unauthorized("Incorrect Password");
                }

                var userId =
                    _dapper.LoadDataSingle<int>(
                        $"SELECT UserId from TutorialAppSchema.Users where Email ='{user.Email}'");
                return Ok(new Dictionary<string, string>
                {
                    {
                        "token", _authHelper.CreateToken(userId)
                    }
                });
            }
            catch (Exception _)
            {
                return BadRequest(_.ToString());
            }
        }

        [HttpGet("RefreshToken")]
        public IActionResult RefreshToken()
        {
            var userId = User.FindFirst("userId")?.Value ?? "";
            var userIdFromDb =
                _dapper.LoadDataSingle<int>($"SELECT UserId FROM TutorialAppSchema.Users WHERE UserId = {userId}");
            return Ok(new Dictionary<string, string> { { "token", _authHelper.CreateToken(userIdFromDb) } });
        }


    }

}