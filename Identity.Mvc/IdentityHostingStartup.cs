using Identity.Mvc.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.Mvc;

public static class IdentityHostingStartup
{
	public static void Configure(this IServiceCollection services)
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
					RequireNonAlphanumeric = true,
					RequireLowercase = true,
					RequireUppercase = true,
					RequireDigit = true,
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
			.AddUserStore<UserStore<IdentityUser<Guid>, IdentityRole<Guid>, AppDbContext, Guid>>()
			//.AddErrorDescriber<IdentityErrorDescriber>()
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
				opt = new CookieAuthenticationOptions
				{
					LoginPath = new PathString("/Account/Login"),
					LogoutPath = new PathString("/Account/Logout"),
					AccessDeniedPath = new PathString("/Account/Login"),
					Cookie = new() { Name = "IdentityCookie" },
					SlidingExpiration = true,
					ExpireTimeSpan = TimeSpan.FromHours(1)
				};
			}
		);
	}
}
