namespace FirstAPI.DTOs
{
    public class UserJobInfoDTO
    {
        public int UserId { get; set; }
        public string JobTitle { get; set; } = "";
        public string Department { get; set; } = "";
    }
}