using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using FirstAPI.DTOs;
using FirstAPI.Data;
using FirstAPI.Models;
using AutoMapper;
namespace FirstAPI.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class UserEFController : ControllerBase
{
    private readonly DataContextEf _entityFrameWork;
    private readonly IMapper _mapper;
    public UserEFController(IConfiguration configuration)
    {
        _entityFrameWork = new DataContextEf(configuration);
        _mapper = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<UserDTO, User>();
        }).CreateMapper();
    }



    [HttpGet("GetUsers")]
    public IEnumerable<User> GetUser()
    {
        return [.. _entityFrameWork.Users];
    }

    [HttpGet("GetUser/{userId}")]
    public User GetUser(int userId)
    {
        return _entityFrameWork.Users.FirstOrDefault(x => x.UserId == userId)?? throw new Exception("User not found");
    }


    [HttpPost("AddUser")]
    public IActionResult AddUser([FromBody] UserDTO user)
    {
        User newUser = _mapper.Map<User>(user);

        _entityFrameWork.Users.Add(newUser);
        _entityFrameWork.SaveChanges();
        return (newUser.UserId != 0) ? Ok() : throw new Exception("Error adding user");

    }

    [HttpPut("UpdateUser")]
    public IActionResult UpdateUser(User user)
    {
        User userToUpdate = _entityFrameWork.Users.FirstOrDefault(x => x.UserId == user.UserId) ?? throw new Exception("User not found");

        userToUpdate.FirstName = user.FirstName;
        userToUpdate.LastName = user.LastName;
        userToUpdate.Email = user.Email;
        userToUpdate.Gender = user.Gender;
        userToUpdate.Active = user.Active;

        _entityFrameWork.SaveChanges();
        return Ok();
    }

    [HttpDelete("DeleteUser/{userId}")]
    public IActionResult DeleteUser(int userId)
    {
        User userToDelete = _entityFrameWork.Users.FirstOrDefault(x => x.UserId == userId) ??  throw new Exception("User not found");
        _entityFrameWork.Users.Remove(userToDelete);
        _entityFrameWork.SaveChanges();
       return (userToDelete.UserId != 0) ? Ok() : throw new Exception("Error deleting user");

    }
}

