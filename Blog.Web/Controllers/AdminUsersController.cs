using Blog.Web.Models.ViewModel;
using Blog.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Web.Controllers
{
    [Authorize(Roles="Admin")]
    public class AdminUsersController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<IdentityUser> _userManager;
        public AdminUsersController(IUserRepository userRepository, UserManager<IdentityUser> userManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
             
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var users = await _userRepository.GetAll();
            var usersViewModel = new UserViewModel();
            usersViewModel.Users = new List<User>();

            foreach (var user in users) {
                
                usersViewModel.Users.Add(
                    new User { 
                        Id = Guid.Parse(user.Id),
                        Username = user.UserName,
                        EmailAddress = user.Email
                    }    
                );


            }



            return View(usersViewModel);
        }



        [HttpPost]
        public async Task<IActionResult> List(UserViewModel request) {



            var identityUser = new IdentityUser
            {
                UserName = request.Username,
                Email = request.Email

            };


            var identityResult = await _userManager.CreateAsync(identityUser, request.Password);

            if (identityResult.Succeeded)
            {
                //assign user role

                var roles = new List<string> { "User" };

                if (request.IsAdmin)
                {

                    roles.Add("Admin");

                }


                var roleIdentityResult = await _userManager.AddToRolesAsync(identityUser, roles);

                if (roleIdentityResult is not null && roleIdentityResult.Succeeded) {

                    return RedirectToAction("List", "AdminUsers");
                }

                  
            }






            return View();
        
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id) {


            
            var deleteUser = await _userManager.FindByIdAsync(id.ToString());

            if (deleteUser is not null) {
                var identityResult = await _userManager.DeleteAsync(deleteUser);

                if (identityResult is not null && identityResult.Succeeded) {
                    return RedirectToAction("List", "AdminUsers");
                }
            }

            return View();

        }
    }
}
