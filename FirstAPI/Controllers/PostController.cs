using System.Data;
using Asp.Versioning;
using Dapper;
using FirstAPI.Data;
using FirstAPI.DTOs;
using FirstAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FirstAPI.Controllers;


[Authorize]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class PostController : ControllerBase
{
    private readonly DataContextDapper _datacontextDapper = DataContextDapper.GetInstance();

    [AllowAnonymous]
    [HttpGet("GetPosts")]
    public IEnumerable<Post> GetPosts(int? postId, [FromQuery] int? userId, [FromQuery] string? searchItem = null, [FromRoute] string version = "1.0")
    {
        const string storedProcedure = "TutorialAppSchema.spPosts_Get";
        var parameters = new DynamicParameters();
        if (postId is > 0)
        {
            parameters.Add("@PostId", postId);
        }
        if (userId is > 0)
        {
            parameters.Add("@UserId", userId);
        }
        if (searchItem is not null)
        {
            parameters.Add("@SearchValue", searchItem);
        }
        return _datacontextDapper.LoadData<Post>(storedProcedure, parameters, CommandType.StoredProcedure);
    }

    [HttpGet("MyPosts")]
    public IEnumerable<Post> MyPosts()
    {
        var userId = User.FindFirst("userId")?.Value;
        if (string.IsNullOrEmpty(userId)) return [];
        const string storedProcedure = "TutorialAppSchema.spPosts_Get";
        var parameters = new DynamicParameters();
        parameters.Add("@UserId", int.Parse(userId));
        return _datacontextDapper.LoadData<Post>(storedProcedure, parameters, CommandType.StoredProcedure);
    }

    [AllowAnonymous]
    [HttpPut("UpsertPost")]
    public IActionResult UpsertPost(PostDto post)
    {
        const string storedProcedure = "TutorialAppSchema.spPosts_Upsert";
        var parameters = new DynamicParameters();
        foreach (var property in post.GetType().GetProperties())
        {
            parameters.Add($"@{property.Name}", property.GetValue(post));
        }
        parameters.Add("@UserId", User.FindFirst("userId")?.Value);
        return _datacontextDapper.ExecuteSql(storedProcedure, parameters, CommandType.StoredProcedure) ? Ok() : BadRequest("Error updating post");
    }


    [HttpDelete("DeletePost/{postId:int}/{userId:int}")]
    public IActionResult DeletePost(int postId, int userId)
    {
        var parameters = new DynamicParameters();
        const string storedProcedure = $"TutorialAppSchema.spPost_Delete";
        parameters.Add("@PostId",postId);
        parameters.Add("@UserId",userId);
        return _datacontextDapper.ExecuteSql(storedProcedure,parameters,CommandType.StoredProcedure) ? Ok($"Post {postId} for user {userId} deleted") : BadRequest("Error deleting post");
    }

}