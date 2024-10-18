using Microsoft.AspNetCore.Mvc;
using FirstAPI.DTOs;
using FirstAPI.Data;
using FirstAPI.Models;
namespace FirstAPI.Controllers;

[ApiController]
[Route("[controller]")] 
public class UserController : ControllerBase
{
   private readonly DataContextDapper _datacontextDapper;
   public UserController(IConfiguration configuration)
    {
        _datacontextDapper = new DataContextDapper(configuration);
    }



    [HttpGet("GetUsers")]
    public IEnumerable<User> GetUser()
    {
        return _datacontextDapper.LoadData<User>($"SELECT * from TutorialAppSchema.Users");
    }

    [HttpGet("GetUser/{UserId}")]
    public User GetUser(int userId)
    {
        return _datacontextDapper.LoadDataSingle<User>($"SELECT * from TutorialAppSchema.Users WHERE UserId = {userId}");
    }

    
    [HttpPost("AddUser")]  
    public IActionResult AddUser([FromBody] UserDTO user)
    {
        var sql = $"INSERT INTO TutorialAppSchema.Users (FirstName, LastName, Email, Gender, Active) VALUES ('{user.FirstName}', '{user.LastName}', '{user.Email}', '{user.Gender}', '{user.Active}')";
        
       return _datacontextDapper.ExecuteSql(sql) ? Ok() : throw new Exception("Error adding user");
    }

    [HttpPut("UpdateUser")]
    public IActionResult UpdateUser(User user)
    {
        string sql = $"UPDATE TutorialAppSchema.Users SET FirstName = '{user.FirstName}', LastName = '{user.LastName}', Email = '{user.Email}', Gender = '{user.Gender}', Active = '{user.Active}' WHERE UserId = {user.UserId}";

        return _datacontextDapper.ExecuteSql(sql) ? Ok() : throw new Exception("Error updating user");
    }

    [HttpDelete("DeleteUser/{UserId}")]
    public IActionResult DeleteUser(int userId)
    {
        string sql = $"DELETE FROM TutorialAppSchema.Users WHERE UserId = {userId}";

        return _datacontextDapper.ExecuteSql(sql) ? Ok() :throw new Exception("Error deleting user");

    }
}

