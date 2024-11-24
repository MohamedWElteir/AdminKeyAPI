using System.Data;
using Asp.Versioning;
using AutoMapper;
using Dapper;
using FirstAPI.Data;
using FirstAPI.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FirstAPI.Helpers;

namespace FirstAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class AuthController : ControllerBase
    {
        private readonly DataContextDapper _datacontextDapper;
        private readonly AuthHelper _authHelper;
        private readonly ReusableSql _reusableSql;
        private readonly IMapper _mapper;
        public AuthController()
        {
            _datacontextDapper = DataContextDapper.GetInstance();
            _authHelper = new AuthHelper();
            _reusableSql = new ReusableSql();
            _mapper = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<UserRegistrationDto, UserDto>()));
        }


        [AllowAnonymous]
        [HttpPost("Register")]
        public IActionResult Register(UserRegistrationDto user, [FromRoute] string version = "1.0") // the database does not have any relationship between the tables
        {
            if (user.Password != user.ConfirmPassword)
            {
                return BadRequest("Passwords do not match");
            }

            var parameters = new DynamicParameters();
            const string storedProcedure = "TutorialAppSchema.spLoginConfirmation_Get";
            parameters.Add("@Email",user.Email);
            var exisingUsersEnumerable = _datacontextDapper.LoadData<object>(storedProcedure,parameters,CommandType.StoredProcedure);

            if (exisingUsersEnumerable.Any()) return Conflict("User already exists");

            var userSetPassword = new UserLoginDto
            {
                Email = user.Email,
                Password = user.Password
            };

            if (!_authHelper.SetPassword(userSetPassword)) return StatusCode(500, "Error adding salt and hash");
            var userDto = _mapper.Map<UserDto>(user);

            return _reusableSql.UpsertUser(userDto) ? Ok("User registered") : throw new Exception("Error registering user");
        }
        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login(UserLoginDto user, [FromRoute] string version = "1.0")
        {
            var sql = $"TutorialAppSchema.spLoginConfirmation_Get";
            var sqlParameters = new DynamicParameters();
            sqlParameters.Add("@Email", user.Email);
            try
            {
                var dbUser = _datacontextDapper.LoadDataSingle<UserForLoginConfirmationDto>(sql, sqlParameters, CommandType.StoredProcedure);

                var passwordHash = _authHelper.GetPasswordHash(user.Password, dbUser.PasswordSalt);
                if(!passwordHash.SequenceEqual(dbUser.PasswordHash))
                {
                   return Unauthorized("Incorrect Password");
                }

                var userId =
                    _datacontextDapper.LoadDataSingle<int>($"SELECT UserId from TutorialAppSchema.Users where Email ='{user.Email}'");
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

        [HttpPut("ResetPassword")]
        public IActionResult ResetPassword(UserLoginDto user, [FromRoute] string version = "1.0")
        {
           return (_authHelper.SetPassword(user)) ? Ok("Password reset") : StatusCode(500, "Error resetting password");
        }

        [HttpGet("RefreshToken")]
        public IActionResult RefreshToken([FromRoute] string version = "1.0")
        {
            var userId = User.FindFirst("userId")?.Value ?? "";
            var userIdFromDb =
                _datacontextDapper.LoadDataSingle<int>($"SELECT UserId FROM TutorialAppSchema.Users WHERE UserId = {userId}");
            return Ok(new Dictionary<string, string> { { "token", _authHelper.CreateToken(userIdFromDb) } });
        }


    }

}