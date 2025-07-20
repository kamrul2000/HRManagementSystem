using HRManagementSystem.Data;
using HRManagementSystem.Interface;
using HRManagementSystem.Models;
using HRManagementSystem.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// 1. Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Add custom services
builder.Services.AddScoped<IAuthService, AuthService>();

// 3. Add Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login"; // ?? Matches your controller
        options.AccessDeniedPath = "/Auth/AccessDenied"; // Optional
    });

// 4. Add MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// 5. Configure middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // ?? Must come BEFORE UseAuthorization
app.UseAuthorization();

// 6. Route setup
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
