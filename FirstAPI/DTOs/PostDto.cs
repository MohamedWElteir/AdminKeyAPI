namespace FirstAPI.DTOs;

public class PostDto
{
    public int? PostId { get; set; }
    public required string PostTitle { get; set; }
    public required string PostContent { get; set; }

}