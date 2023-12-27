using BaiTapLon.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("MyConnect");
builder.Services.AddDbContext<BanHangDbContext>(option => option.UseSqlServer(connectionString));

builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
//Dung session
builder.Services.AddSession(option =>
{
    option.IdleTimeout = TimeSpan.FromSeconds(300);
    option.Cookie.HttpOnly = true;
    option.Cookie.IsEssential = true;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=SanPhams}/{action=Index}/{id?}");

app.Run();
