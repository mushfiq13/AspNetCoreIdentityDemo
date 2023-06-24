using Identity.Mvc.Data;
using Identity.Mvc.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.Mvc;

public static class IdentityConfig
{
	public static void ConfigureIdentity(this IServiceCollection services)
	{
		services
			.AddScoped<RegistrationViewModel>()
			.AddScoped<LoginViewModel>()
			.AddDbContext<AppDbContext>(opt =>
			{
				opt.UseSqlServer(
					@"Data Source=.\SQLEXPRESS;Database=practice_db;Trusted_Connection=True;TrustServerCertificate=True;");
			})
			.AddIdentity<IdentityUser<Guid>, IdentityRole<Guid>>()
			//.AddUserManager<UserManager<IdentityUser<Guid>>>()
			//.AddSignInManager<SignInManager<IdentityUser<Guid>>>()
			//.AddPasswordValidator<PasswordValidator<IdentityUser<Guid>>>()
			.AddEntityFrameworkStores<AppDbContext>()
			.AddDefaultTokenProviders();
	}
}
