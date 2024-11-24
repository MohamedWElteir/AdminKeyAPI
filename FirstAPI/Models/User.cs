using System;
namespace FirstAPI.Models
{
   public partial class User
   {

      public int UserId { get; set; }
      public required string FirstName { get; set; }
      public required string LastName { get; set; }
      public required string Email { get; set; }
      public required string Gender { get; set; }
      public bool Active { get; set; }
      public required string JobTitle { get; set; }
      public required string Department { get; set; }
      public decimal Salary { get; set; }
      public decimal AvgSalary { get; set; }

   }
}