using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore;
using SWP391_Gr3.Models;
using SWP391_Gr3.Repositories;
using SWP391_Gr3.Services;
using SWP391_Gr3.Util;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<Swp391Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyCnn")));

builder.Services.AddScoped<SWP391_Gr3.Services.IUserService, SWP391_Gr3.Services.UserService>(); 
builder.Services.AddScoped<SWP391_Gr3.Repositories.IUserRepository, SWP391_Gr3.Repositories.UserRepository>();

builder.Services.AddScoped<SWP391_Gr3.Services.IRoleSer, SWP391_Gr3.Services.RoleSer>();
builder.Services.AddScoped<SWP391_Gr3.Repositories.IRoleRepo, SWP391_Gr3.Repositories.RoleRepo>();
var emailSettings = builder.Configuration
    .GetSection("EmailSettings")
    .Get<EmailSettings>();

builder.Services.AddSingleton(emailSettings);
builder.Services.AddTransient<IEmailService, EmailService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
