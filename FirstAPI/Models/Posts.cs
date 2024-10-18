namespace FirstAPI.Models;

public partial class Posts
{
    public int PostId { get; init; }
    public int UserId { get; set; }
    public string PostTitle { get; set; } = "";
    public string PostContent { get; set; } = "";
    public DateTime PostCreated { get; set; }
    public DateTime PostUpdated { get; set; }


}