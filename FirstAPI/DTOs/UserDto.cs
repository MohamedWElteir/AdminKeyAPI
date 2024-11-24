namespace FirstAPI.DTOs
{
   public class UserDto
   {

       public required string FirstName { get; init; }
       public required string LastName { get; init; }
       public required string Email { get; init; }
       public required string Gender { get; init; }
       public required string JobTitle { get; init; }
       public required string Department { get; init; }
       public decimal Salary { get; init; }
       public bool Active { get; init; } = true;
       public int UserId { get; init; }

   }
}