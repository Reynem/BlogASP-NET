using Blog.Data;
using Blog.Models;
using Blog.Tests;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseSqlite("Data Source=users.db"));

builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
    .AddEntityFrameworkStores<UserDbContext>()
    .AddDefaultTokenProviders(); 

builder.Services.AddDbContext<BlogDbContext>();

builder.Services.AddServerSideBlazor();
builder.Services.AddRazorPages();
builder.Services.AddHttpClient("Blog", client =>
{
    client.BaseAddress = new Uri("https://localhost:5000/");
});

builder.Services.AddControllers();
builder.Services.AddControllersWithViews();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseStaticFiles();

app.MapRazorPages();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
