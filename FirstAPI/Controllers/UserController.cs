using System.Data;
using Asp.Versioning;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using FirstAPI.DTOs;
using FirstAPI.Data;
using FirstAPI.Helpers;
using FirstAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OutputCaching;

namespace FirstAPI.Controllers;


[Authorize]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]

public class UserController : ControllerBase
{
   private readonly DataContextDapper _datacontextDapper;
   private readonly ReusableSql _reusableSql;
   public UserController()
    {
        _datacontextDapper = DataContextDapper.GetInstance();
        _reusableSql = new ReusableSql();
    }


    [AllowAnonymous]
    [HttpGet("GetUsers")]
    [OutputCache]
    public IEnumerable<User> GetUser([FromQuery] int? userId = null, [FromQuery] bool? isActive = null , [FromRoute] string version = "1.0")
    {
        const string storedProcedure = "TutorialAppSchema.spUsers_Get";
        var parameters = new DynamicParameters();
        if (userId is > 0)
        {
            parameters.Add("@UserId", userId);
        }
        if (isActive.HasValue)
        {
            parameters.Add("@Active", isActive);
        }

        return _datacontextDapper.LoadData<User>(storedProcedure, parameters, CommandType.StoredProcedure);
    }




    [HttpPut("UpsertUser")]
    public IActionResult UpsertUser(UserDto user, [FromRoute] string version = "1.0")
    {
        return _reusableSql.UpsertUser(user) ? Ok() : BadRequest("Error adding user");
    }

    [HttpDelete("DeleteUser/{userId}")]
    public IActionResult DeleteUser(int userId, [FromRoute] string version = "1.0")
    {
       const string storedProcedure = $"TutorialAppSchema.spUser_Delete";
       var parameters = new DynamicParameters();
       parameters.Add("@UserId",userId);

        return _datacontextDapper.ExecuteSql(storedProcedure,parameters,CommandType.StoredProcedure) ? Ok($"User {userId} deleted") :BadRequest("Error deleting user");

    }
}

