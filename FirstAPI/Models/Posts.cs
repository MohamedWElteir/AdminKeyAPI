namespace FirstAPI.Models;

public partial class Posts
{
    public required int PostId { get; init; }
    public required int UserId { get; set; }
    public required string PostTitle { get; set; }
    public required string PostContent { get; set; }
    public DateTime PostCreated { get; set; }
    public DateTime PostUpdated { get; set; }


}