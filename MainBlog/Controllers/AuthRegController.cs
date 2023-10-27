using Blog;
using MainBlog.Models;
using MainBlog.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

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
            User user = null;
            if (ModelState.IsValid)
            {
                user = await _userManager.FindByEmailAsync(viewModel.Email);
                if (user == null)
                {
                    user = new User();
                    user.UserName = viewModel.Name;
                    user.Age = viewModel.Age;
                    user.Email = viewModel.Email;
                    user.PasswordHash = PasswordHash.HashPassword(viewModel.ComparePassword);
                    user.RegistrationDate = DateTime.UtcNow;

                    var result = await _userManager.CreateAsync(user, user.PasswordHash);

                    if (result.Succeeded)
                    {
                        string[] roles = new[] { "Administrator", "Moderator", "User" };
                        await _userManager.AddToRoleAsync(user, roles[0]);
                        await _signInManager.SignInAsync(user, false);

                        Response.Cookies.Append("RegisteredUsername", user.UserName); //запись в куки
                        string logFile = Path.Combine(_env.ContentRootPath, "Logs", "RegistrationLogs.txt");
                        using (StreamWriter sw = new StreamWriter(logFile, true))
                        {

                            sw.WriteLineAsync($"{viewModel.Name} зарегестрировался в {user.RegistrationDate}");

                            sw.Close();
                        }
                        return RedirectToAction("GreetingPage", "Home");
                    }
                    // Сохраним имя пользователя в TempData
                    //TempData["RegisteredUsername"] = user.Name;
                    // Или сохраним имя пользователя в куки
                }
            }
            return View(viewModel);
        }
        #endregion
        






        #region Login
        [HttpGet]
        public IActionResult Login()//(string rtrnUrl)
        {
            //ViewBag.returnUrl = rtrnUrl;
            //return RedirectToAction("Login", "AuthReg");
            return View();// View(new LoginViewModel { ReturnUrl = rtrnUrl });
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)//, string retrnUrl)
        {
            string filePath22 = Path.Combine(_env.ContentRootPath, "Logs", "LoginStateLogs.txt");
            if (ModelState.IsValid)
            {                
                using (StreamWriter fs = new StreamWriter(filePath22, true))
                {
                    fs.WriteLineAsync($"{DateTime.UtcNow} ModelState.IsValid!");
                    fs.Close();
                }
                
                User user = await _userManager.FindByEmailAsync(model.Email);

                if (user == null || !await _userManager.CheckPasswordAsync(user, PasswordHash.HashPassword(model.Password)))
                {
                    string filePath = Path.Combine(_env.ContentRootPath, "Logs", "LoginLogs.txt");
                    using (StreamWriter fs = new StreamWriter(filePath, true))
                    {
                        fs.WriteLineAsync($"{DateTime.UtcNow} Неудачная попытка залогиниться! Почта: {model.Email}, пароль {PasswordHash.HashPassword(model.Password)}");
                        fs.Close();
                    }
                    
                    return Content($"{new Exception("Пользователь не найден!")}");
                }
                await _signInManager.SignOutAsync(); // аннулирует любой имеющийся у пользователя сеанс????
                Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signInManager.PasswordSignInAsync(user, PasswordHash.HashPassword(model.Password), false, false);//проводит уже саму аутентификацию. Второй false - должна ли учётка блокироваться в случае некорректного пароля
                if (signInResult.Succeeded)
                {
                    Response.Cookies.Append("LoginUsername", user.UserName); //запись в куки
                    //return Redirect(retrnUrl ?? "/");
                    return RedirectToAction("GreetingPage", "Home");
                }
                //var claims = new List<Claim>
                //{
                //    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                //    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.RoleType)
                //};
                //var user = _mapper.Map<User>(model);

                //var result = await _signInManager.PasswordSignInAsync(user.Email, model.Password, model.RememberMe, false);
                return RedirectToAction("UserPosts", "Blog");
                //return RedirectToAction("Index", "Home");
            }
            else if (String.IsNullOrEmpty(model.Email) || String.IsNullOrEmpty(model.Password))
                throw new ArgumentNullException("Запрос не корректен");
            else
            {
                ModelState.AddModelError("", "Неправильный логин и (или) пароль");
            }
            using (StreamWriter fs = new StreamWriter(filePath22, true))
            {
                fs.WriteLineAsync($"{DateTime.UtcNow} ModelState.IsValid = false!");
                fs.Close();
            }
            return RedirectToAction("Index", "Home");
        }
        #endregion
        #region Logout

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme); //очистка Cookie'сов

            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        #endregion

    }
}


