using Microsoft.AspNetCore.Mvc;
using FirstAPI.DTOs;
using FirstAPI.Data;
using FirstAPI.Models;
namespace FirstAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserJobInfoController : ControllerBase
{
    DataContextDapper _datacontextDapper;
    public UserJobInfoController(IConfiguration configuration)
    {
        _datacontextDapper = new DataContextDapper(configuration);
    }



    [HttpGet("GetUserJobInfo")]
    public IEnumerable<UserJobInfo> GetUserJobInfo()
    {
        return _datacontextDapper.LoadData<UserJobInfo>($"SELECT * from TutorialAppSchema.UserJobInfo");
    }
    [HttpGet("GetUserJobInfo/{UserId}")]
    public UserJobInfo GetUserJobInfo(int UserId)
    {
        return _datacontextDapper.LoadDataSingle<UserJobInfo>($"SELECT * from TutorialAppSchema.UserJobInfo WHERE UserId = {UserId}");
    }

    [HttpPost("AddUserJobInfo")]
    public IActionResult AddUserJobInfo([FromBody] UserJobInfoDTO userJobInfo)
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

