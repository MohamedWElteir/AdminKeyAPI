using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using FirstAPI.DTOs;
using FirstAPI.Data;
using FirstAPI.Models;
namespace FirstAPI.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]

public class UserJobInfoController : ControllerBase
{
    private readonly DataContextDapper _datacontextDapper = DataContextDapper.GetInstance();


    [HttpGet("GetUserJobInfo")]
    public IEnumerable<UserJobInfo> GetUserJobInfo()
    {
        return _datacontextDapper.LoadData<UserJobInfo>($"SELECT * from TutorialAppSchema.UserJobInfo");
    }
    [HttpGet("GetUserJobInfo/{userId}")]
    public UserJobInfo GetUserJobInfo(int userId)
    {
        return _datacontextDapper.LoadDataSingle<UserJobInfo>($"SELECT * from TutorialAppSchema.UserJobInfo WHERE UserId = {userId}");
    }

    [HttpPost("AddUserJobInfo")]
    public IActionResult AddUserJobInfo([FromBody] UserJobInfoDto userJobInfo)
    {
        string sql = $"INSERT INTO TutorialAppSchema.UserJobInfo (UserId, JobTitle, Department) VALUES ('{userJobInfo.UserId}', '{userJobInfo.JobTitle}', '{userJobInfo.Department}')";

        return _datacontextDapper.ExecuteSql(sql) ? Ok() : throw new Exception("Error adding user job info");
    }

    [HttpPut("UpdateUserJobInfo")]
    public IActionResult UpdateUserJobInfo(UserJobInfo userJobInfo)
    {
        string sql = $"UPDATE TutorialAppSchema.UserJobInfo SET JobTitle = '{userJobInfo.JobTitle}', Department = '{userJobInfo.Department}' WHERE UserId = {userJobInfo.UserId}";

        return _datacontextDapper.ExecuteSql(sql) ? Ok() : throw new Exception("Error updating user job info");
    }

    [HttpDelete("DeleteUserJobInfo/{UserId}")]
    public IActionResult DeleteUserJobInfo(int UserId)
    {
        string sql = $"DELETE FROM TutorialAppSchema.UserJobInfo WHERE UserId = {UserId}";

        return _datacontextDapper.ExecuteSql(sql) ? Ok() : throw new Exception("Error deleting user job info");

    }


}

