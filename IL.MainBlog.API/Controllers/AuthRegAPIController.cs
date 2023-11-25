using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;
using MainBlog.ViewModels;
using MainBlog.Data.Models;

namespace MainBlog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthRegAPIController : ControllerBase
    {

		private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private IWebHostEnvironment _env;
        public AuthRegAPIController(UserManager<User> userManager, SignInManager<User> signInManager, IWebHostEnvironment environment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _env = environment;
        }
        #region RegistrateUser
        [HttpGet("RegistrateUser")]
        public IActionResult RegistrateUser()
        {
            return Ok(StatusCode(200));
        }

        /// <summary>
        /// Регистрация пользователя
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns>viewModel</returns>
        [HttpPost("RegistrateUser")]
        public async Task<IActionResult> RegistrateUser(RegistrateViewModel viewModel)
        {
            User user = null!;
            if (ModelState.IsValid)
            {
                user = await _userManager.FindByEmailAsync(viewModel.Email);
                if (user == null)
                {
                    user = new User();
                    user.UserName = viewModel.Name;
                    user.Age = viewModel.Age;
                    user.Email = viewModel.Email;
                    user.PasswordHash = viewModel.ComparePassword;
                    user.RegistrationDate = DateTime.UtcNow;

                    var result = await _userManager.CreateAsync(user, user.PasswordHash);

                    if (result.Succeeded)
                    {
                        string[] roles = new[] { "Administrator", "Moderator", "User" };
                        await _userManager.AddToRoleAsync(user, roles[0]);
                        await _signInManager.SignInAsync(user, false);                        
                        return RedirectToAction("GreetingPage", "Home");
                    }
                }
            }
            return Ok(viewModel);
        }
        #endregion
        

        
        #region Login
        [HttpGet("Login")]
        public IActionResult Login(/*string rtrnUrl*/)
        {
            return Ok(/*new LoginViewModel { ReturnUrl = rtrnUrl }*/);
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {            
            if (ModelState.IsValid)
            {
                foreach (var elem in ModelState)
                {
                    var k = elem.Key;
                    var v = elem.Value;
                }
                User user = await _userManager.FindByEmailAsync(model.Email);

                if (user == null)
                {
                    return StatusCode(400);
				}
                await _signInManager.SignOutAsync(); // аннулирует любой имеющийся у пользователя сеанс????
                SignInResult signInResult = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);//проводит уже саму аутентификацию. Второй false - должна ли учётка блокироваться в случае некорректного пароля
                if (signInResult.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);                    
                    return Ok(string.Format("Hello {@user}", user));
                }
                else 
                {
                    return StatusCode(400);
                }
            }
            return StatusCode(404);
        }
        #endregion
        #region Logout

        [HttpPost("Logout")]
        [ValidateAntiForgeryToken] //ValidateAntiForgeryToken будет работать только, с HttpPost, причем в cshtml надо указать именно <form method="post">
        public async Task<IActionResult> Logout()
        {
            User a = await _signInManager.UserManager.GetUserAsync(User);
            await _signInManager.SignOutAsync();

            return Ok("Пользовытель вышел");

        }
        #endregion
        
    }
}


/*
  "name": "user2",
  "age": 22,
  "email": "user2@mail.ru",
  "password": "2222",
  "comparePassword": "2222"
 */