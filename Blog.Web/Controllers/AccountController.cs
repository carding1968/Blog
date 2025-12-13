using Blog.Web.Models.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager; 
        private readonly SignInManager<IdentityUser> _signInManager;
        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel) {

            var identityUser = new IdentityUser {
                UserName = registerViewModel.Username,
                Email = registerViewModel.Email

            };


            var identityResult = await _userManager.CreateAsync(identityUser, registerViewModel.Password);

            if (identityResult.Succeeded) {
                //assign user role

                var roleIdentityResult = await _userManager.AddToRoleAsync(identityUser, "User");

                if (roleIdentityResult.Succeeded) {

                    //Show Success notif
                    return RedirectToAction("Register");
                }
            }

            //show error
            return View("Register");

        }

        [HttpGet]
        public IActionResult Login(string returnUrl) { 
        
            var model = new LoginViewModel { ReturnUrl = returnUrl };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel) {

            var signInResult = await _signInManager.PasswordSignInAsync(loginViewModel.Username, loginViewModel.Password, false, false);

            if (signInResult != null && signInResult.Succeeded) {

                if (!string.IsNullOrWhiteSpace(loginViewModel.ReturnUrl)) {
                    return Redirect(loginViewModel.ReturnUrl);
                }

                return RedirectToAction("Index", "Home");
            
            }

            //show error
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Logout() { 
        
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied() {

            return View();
        }



    }
}
