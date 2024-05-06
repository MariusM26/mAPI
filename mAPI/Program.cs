using mAPI.Database;
using mAPI.Database.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Security.Claims;

Log.Logger = new LoggerConfiguration().CreateLogger();


const string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

var currentLocation = Directory.GetCurrentDirectory();

builder.Configuration
    .SetBasePath(currentLocation)
    .AddJsonFile(Path.Combine(currentLocation, "appsettings.json"), optional: false, reloadOnChange: true);

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DevConnection")));

builder.Services.AddAuthorization();

builder.Services.AddIdentityApiEndpoints<ApplicationUser>().AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:3000", "https://localhost:5187", "http://localhost:5187")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

builder.Services.AddControllers().AddApplicationPart(typeof(Program).Assembly);



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapPost("/logout", async (SignInManager<ApplicationUser> signInManager) =>
{
    await signInManager.SignOutAsync();
    return Results.Ok();

}).RequireAuthorization();


app.MapGet("/pingauth", (ClaimsPrincipal user) =>
{
    var email = user.FindFirstValue(ClaimTypes.Email);

    return Results.Json(new
    {
        Email = email
    });

}).RequireAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapControllers();
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseCors(MyAllowSpecificOrigins);
app.MapIdentityApi<ApplicationUser>();

app.UseHttpsRedirection();
app.UseAuthorization();
app.Run();


namespace mAPI
{
    public partial class Program
    {

    }
}