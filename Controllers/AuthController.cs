using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Blog.Models;
using Blog.ViewModels;

namespace Blog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return await Task.FromResult<IActionResult>(BadRequest(ModelState));
            }
            var user = new User { UserName = GenerateUserName(), Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return await Task.FromResult<IActionResult>(Ok());
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return await Task.FromResult<IActionResult>(BadRequest(ModelState));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginViewModel model) 
        {
            if (!ModelState.IsValid)
            {
                return await Task.FromResult<IActionResult>(BadRequest(ModelState));
            }
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return await Task.FromResult<IActionResult>(Ok());
            }
            if (result.IsLockedOut)
            {
                return await Task.FromResult<IActionResult>(Forbid("User is locked out"));
            }
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return await Task.FromResult<IActionResult>(BadRequest(ModelState));

        }

        [HttpPut]
        public async Task<IActionResult> ChangeProfile(ProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return await Task.FromResult<IActionResult>(BadRequest(ModelState));
            }
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return await Task.FromResult<IActionResult>(NotFound("User not found."));
            }
            user.UserName = model.UserName ?? user.UserName;
            user.BirthDate = model.BirthDate ?? user.BirthDate;
            user.Bio = model.Bio ?? " ";
            user.ProfilePictureUrl = model.ProfilePictureUrl ?? " ";
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return await Task.FromResult<IActionResult>(Ok());
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return await Task.FromResult<IActionResult>(BadRequest(ModelState));
        }

        private static string GenerateUserName()
        {
            Random random = new Random();
            return "Anon" + random.Next(1000, 10000);
        }
    }
}
