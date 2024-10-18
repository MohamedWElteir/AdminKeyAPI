using System.Data;
using System.Text;
using FirstAPI.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: "DevCors",
        corsPolicyBuilder =>
        {
            corsPolicyBuilder.WithOrigins("http://localhost:3000", "https://localhost:4200", "http://localhost:8000") // ports for react, angular, vue
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
        }
    );
    options.AddPolicy(
       name: "ProdCors",
       corsPolicyBuilder =>
       {
              corsPolicyBuilder.WithOrigins("https://myproduction.com") // Add your production domain here
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
       }
   );

});

builder.Services.AddScoped<DataContextDapper>();

SymmetricSecurityKey key = new(
    Encoding.UTF8.GetBytes(
        builder.Configuration.GetSection("AppSettings:TokenKey").Value ?? throw new Exception("TokenKey is null")
    )
);

TokenValidationParameters tokenValidationParameters = new()
{
    IssuerSigningKey = key,
    ValidateIssuer = false,
    ValidateIssuerSigningKey = false,
    ValidateAudience = false
};

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = tokenValidationParameters;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors("DevCors");
    app.UseSwagger();
    app.UseSwaggerUI();
}else
{
    app.UseCors("ProdCors");
    app.UseHttpsRedirection();

}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


// dotnet watch -lp https



app.Run();


/*
MEMO FOR THINGS I LEARNED SO FAR:
- [ApiController] attribute is used to indicate that the class is a controller.
- [Route] attribute is used to define the route for the controller.
- ControllerBase is a base class for MVC controllers. (A SMALLER VERSION OF CONTROLLER)
- IConfiguration is used to access the configuration settings.
- GetConnectionString() method is used to get the connection string from the appsettings.json file. // I NEED TO SEE MORE ABOUT HTTPS
- [HttpGet] attribute is used to define the HTTP GET method.
- I HAVE TO MAKE THE MODEL'S PROPERTIES MATCH THE DATABASE'S COLUMN NAMES

*/

/*
THINGS I WANT TO KNOW WHY:
- WHY WHEN I WAS ON HTTPS, IT SAYS MULTIPLE CONSTRUCTORS FOUND, BUT WHEN I WAS ON HTTP, IT WORKS FINE?
*/

/*
THINGS I WANT TO LEARN MORE ABOUT:
- IConfiguration
- Dependency Injection
- HTTPS
- CORS
- HOW NAMESPACES WORK
*/
