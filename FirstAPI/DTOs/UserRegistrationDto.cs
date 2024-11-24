namespace FirstAPI.DTOs
{
    public partial class UserRegistrationDto
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";

        public string Gender { get; set; } = "";
        public bool Active { get; set; } = true;
        public required string JobTitle { get; set; }
        public required string Department { get; set; }
        public decimal Salary { get; set; }
        public string Password { get; set; } = "";
        public string ConfirmPassword { get; set; } = "";
    }
}