using System.Data;
using Asp.Versioning;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using FirstAPI.DTOs;
using FirstAPI.Data;
using FirstAPI.Models;
namespace FirstAPI.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class UserSalaryController : ControllerBase
{
    DataContextDapper _datacontextDapper;
    public UserSalaryController()
    {
        _datacontextDapper = DataContextDapper.GetInstance();
    }




    [HttpGet("GetUserSalaryInfo")]
    public IEnumerable<UserSalary> GetUserJobInfo([FromQuery] int? userId)
    {
        const string storedProcedure = "TutorialAppSchema.spUserSalary_Get";
        var parameters = new DynamicParameters();
        if (userId is > 0)
        {
            parameters.Add("@UserId", userId);
        }
        return _datacontextDapper.LoadData<UserSalary>(storedProcedure, parameters, CommandType.StoredProcedure);
    }

    [HttpPost("AddUserSalaryInfo")]
    public IActionResult AddUserJobInfo([FromBody] UserSalaryDto userSalary) // is it logical to add salary without adding user first ?
    {
        string sql = $"INSERT INTO TutorialAppSchema.UserSalary (Salary) VALUES ('{userSalary.Salary}')";  

        return _datacontextDapper.ExecuteSql(sql) ? Ok() : throw new Exception("Error adding user salary info");
    }

    [HttpPut("UpdateUserUpdateInfo")]
    public IActionResult UpdateUserJobInfo(UserSalary userSalary)
    {
        string sql = $"UPDATE TutorialAppSchema.UserSalary SET Salary = '{userSalary.Salary}' WHERE UserId = {userSalary.UserId}";
        return _datacontextDapper.ExecuteSql(sql) ? Ok() : throw new Exception("Error updating user salary info");
    }

    [HttpDelete("DeleteUserSalaryInfo/{UserId}")]
    public IActionResult DeleteUserSalaryInfo(int UserId) // same logical question
    {
        string sql = $"DELETE FROM TutorialAppSchema.UserSalary WHERE UserId = {UserId}";

        return _datacontextDapper.ExecuteSql(sql) ? Ok() : throw new Exception("Error deleting user salary info"); 
    }


}

