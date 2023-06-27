using Identity.Mvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Mvc.Controllers
{
	public class AccountController : Controller
	{
		private readonly ILogger<AccountController> _logger;
		private readonly UserManager<IdentityUser<Guid>> _userManager;
		private readonly SignInManager<IdentityUser<Guid>> _signInManager;

		public AccountController(
			ILogger<AccountController> logger,
			UserManager<IdentityUser<Guid>> userManager,
			SignInManager<IdentityUser<Guid>> signInManager)
		{
			_logger = logger;
			_userManager = userManager;
			_signInManager = signInManager;
		}

		public IActionResult Index(Guid id)
		{
			return View();
		}


		public IActionResult Register()
		{
			return View();
		}

		[HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(RegistrationViewModel model)
		{
			if (ModelState.IsValid)
			{
				IdentityUser<Guid> newUser = new()
				{
					Id = new Guid(),
					UserName = model.UserName,
					Email = model.Email
				};
				IdentityResult result = await _userManager.CreateAsync(newUser, model.Password);

				if (result.Succeeded)
				{
					await _signInManager.SignInAsync(newUser, isPersistent: false);
					_logger.LogInformation(3, "User created a new account with password.");

					return RedirectToAction(nameof(AccountController.Index));
				}
				else
				{
					_logger.LogError("Error: ", result.Errors);
				}
			}

			// something failed
			return View();
		}

		public IActionResult SignIn()
		{
			return View();
		}

		[HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
		public async Task<IActionResult> SignIn(LoginViewModel model, string returnUrl = null!)
		{
			ViewData["ReturnUrl"] = returnUrl;

			if (ModelState.IsValid)
			{
				var result = await _signInManager.PasswordSignInAsync(
					userName: model.UserName,
					password: model.Password,
					isPersistent: false,
					// To enable password failures to trigger account lockout, set lockoutOnFailure: true
					lockoutOnFailure: false);

				if (result.Succeeded)
				{
					return RedirectToAction(nameof(AccountController.Index));
				}
				else
					_logger.LogError("Error: Invalid Login");
			}

			// something failed
			return View();
		}

		[HttpPost, ValidateAntiForgeryToken]
		public async Task<IActionResult> LogOff()
		{
			// clears the users claims stored in a cookie
			await _signInManager.SignOutAsync();

			return RedirectToAction(nameof(AccountController.Index));
		}
	}
}