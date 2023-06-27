using Identity.Mvc.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.Mvc;

public static class IdentityConfig
{
	public static void ConfigureIdentity(this IServiceCollection services)
	{
		services.AddDbContext<AppDbContext>(opt =>
		{
			opt.UseSqlServer(
				@"Data Source=.\SQLEXPRESS;Database=practice_db;Trusted_Connection=True;TrustServerCertificate=True;");
		});
		services.AddIdentity<IdentityUser<Guid>, IdentityRole<Guid>>(identityOptions =>
			{
				identityOptions.Password = new PasswordOptions
				{
					RequireNonAlphanumeric = false,
					RequireLowercase = false,
					RequireUppercase = false,
					RequireDigit = false,
					RequiredLength = 6,
					RequiredUniqueChars = 0
				};

				identityOptions.Lockout = new LockoutOptions
				{
					MaxFailedAccessAttempts = 5,
					DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5),
					AllowedForNewUsers = true
				};

				identityOptions.User = new UserOptions
				{
					AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyz",
					RequireUniqueEmail = true
				};
			})
			.AddUserManager<UserManager<IdentityUser<Guid>>>()
			.AddRoleManager<RoleManager<IdentityRole<Guid>>>()
			.AddSignInManager<SignInManager<IdentityUser<Guid>>>()
			.AddEntityFrameworkStores<AppDbContext>()
			.AddDefaultTokenProviders();

		services.Configure<IdentityOptions>(configureOptions: opt =>
		{
			// We can also configure Identity Options here...
		});

		services.AddAuthentication().AddCookie(
			// Add Cookie based authentication
			authenticationScheme: CookieAuthenticationDefaults.AuthenticationScheme,
			configureOptions: opt =>
			{
				opt.LoginPath = new PathString("/Account/Login");
				opt.LogoutPath = new PathString("/Account/Logout");
				opt.AccessDeniedPath = new PathString("/Account/Login");
				opt.Cookie.Name = "IdentityCookie";
				opt.SlidingExpiration = true;
				opt.ExpireTimeSpan = TimeSpan.FromHours(1);
			}
		);
	}
}
