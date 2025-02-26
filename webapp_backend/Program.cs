using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using webapp_backend.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Session;
using Microsoft.Extensions.DependencyInjection;
using System;

var builder = WebApplication.CreateBuilder(args);

/* Adding session services with SQL Server storage
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
*/

// Getting connection string from appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//register DbContext with SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(connectionString));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<UserService>();

var app = builder.Build();

/*
app.UseSession(); //Enable session management

app.MapGet("/", async httpContext =>
{
    //Store data in session 
    httpContext.Session.SetString("UserName", "");
    await httpContext.Response.WriteAsync("Session stored in memory!");
});

app.MapGet("/get-session", async httpContext =>
{
    //Retrieve session data 
    var user = httpContext.Session.GetString("UserName") ?? "No session found";
    await httpContext.Response.WriteAsync($"User: {user}");
});
*/

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
