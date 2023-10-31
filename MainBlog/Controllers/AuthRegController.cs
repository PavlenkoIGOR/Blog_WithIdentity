using Blog;
using MainBlog.Models;
using MainBlog.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace MainBlog.Controllers
{
    public class AuthRegController : Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private IWebHostEnvironment _env;
        public AuthRegController(UserManager<User> userManager, SignInManager<User> signInManager, IWebHostEnvironment environment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _env = environment;
        }
        #region RegistrateUser
        [HttpGet]
        public IActionResult RegistrateUser()
        {
            return View();
        }

        [HttpPost]
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

                        //Response.Cookies.Append("RegisteredUsername", user.UserName); //запись в куки
                        string logFile = Path.Combine(_env.ContentRootPath, "Logs", "RegistrationLogs.txt");
                        using (StreamWriter sw = new(logFile, true))
                        {

                            await sw.WriteLineAsync($"{viewModel.Name} зарегестрировался в {user.RegistrationDate}");

                            sw.Close();
                        }
                        return RedirectToAction("GreetingPage", "Home");
                    }
                    // Сохраним имя пользователя в TempData
                    //TempData["RegisteredUsername"] = user.Name;
                    // Или сохраним имя пользователя в куки
                }
                else
                {
                    
                }
            }
            return View(viewModel);
        }
        #endregion
        






        #region Login
        [HttpGet]
        public IActionResult Login()//string rtrnUrl)
        {
            return View();//new LoginViewModel { ReturnUrl = rtrnUrl });
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)//, string retrnUrl)
        {
            string filePath22 = Path.Combine(_env.ContentRootPath, "Logs", "LoginStateLogs.txt");
            if (ModelState.IsValid)
            {                
                using (StreamWriter fs = new(filePath22, true))
                {
                    await fs.WriteLineAsync($"{DateTime.UtcNow} ModelState.IsValid!");
                    fs.Close();
                }
                
                User user = await _userManager.FindByEmailAsync(model.Email);

                if (user == null)// || !await _userManager.CheckPasswordAsync(user, PasswordHash.HashPassword(model.Password)))
                {
                    string filePath = Path.Combine(_env.ContentRootPath, "Logs", "LoginLogs.txt");
                    using (StreamWriter fs = new(filePath, true))
                    {
                        await fs.WriteLineAsync($"{DateTime.UtcNow} Неудачная попытка залогиниться! Почта: {model.Email}, пароль {PasswordHash.HashPassword(model.Password)}");
                        fs.Close();
                    }
                    
                    return Content($"{new Exception("Пользователь не найден!")}");
                }
                //await _signInManager.SignOutAsync(); // аннулирует любой имеющийся у пользователя сеанс????
                SignInResult signInResult = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);//проводит уже саму аутентификацию. Второй false - должна ли учётка блокироваться в случае некорректного пароля
                if (signInResult.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    //Response.Cookies.Append("LoginUsername", user.UserName); //запись в куки

                    return RedirectToAction("GreetingPage", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и/или пароль");
                }

                //var result = await _signInManager.PasswordSignInAsync(user.Email, model.Password, model.RememberMe, false);
                return RedirectToAction("UserPosts", "Blog");
            }
            else if (String.IsNullOrEmpty(model.Email) || String.IsNullOrEmpty(model.Password))
                throw new ArgumentNullException(model.Email);
            else
            {
                ModelState.AddModelError("", "Неправильный логин и (или) пароль");
            }
            using (StreamWriter fs = new(filePath22, true))
            {
                await fs.WriteLineAsync($"{DateTime.UtcNow} ModelState.IsValid = false!");
                fs.Close();
            }
            return RedirectToAction("Index", "Home");
        }
        #endregion
        #region Logout

        [HttpPost]
        [ValidateAntiForgeryToken] //ValidateAntiForgeryToken будет работать только, с HttpPost, причем в cshtml надо указать именно <form method="post">
        public async Task<IActionResult> Logout()
        {
            User a = await _signInManager.UserManager.GetUserAsync(User);

            string logFile = Path.Combine(_env.ContentRootPath, "Logs", "LogOutLogs.txt");
            using (StreamWriter sw = new(logFile, true))
            {

                await sw.WriteLineAsync($"{a.Email} выпилился в {DateTime.UtcNow}");

                sw.Close();
            }
            await _signInManager.SignOutAsync();
            //await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme); //очистка Cookie'сов
            return RedirectToAction("Index", "Home");
        }
        #endregion

    }
}


