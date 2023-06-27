using Identity.Mvc;
using Identity.Mvc.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.ConfigureIdentity();
builder.Services
	.AddScoped<RegistrationViewModel>()
	.AddScoped<LoginViewModel>();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Account}/{action=Index}/{id?}");

app.Run();
