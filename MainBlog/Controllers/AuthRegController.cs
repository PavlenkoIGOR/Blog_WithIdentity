using Blog;
using MainBlog.Models;
using MainBlog.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace MainBlog.Controllers
{
    public class AuthRegController : Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private IWebHostEnvironment _webHostEnvironment;
        public AuthRegController(UserManager<User> userManager, SignInManager<User> signInManager, IWebHostEnvironment environment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _webHostEnvironment = environment;
        }

        [HttpGet]
        public IActionResult RegistrateUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegistrateUser(RegistrateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                User user = new User();
                user.UserName = viewModel.Name;
                user.Age = viewModel.Age;
                user.Email = viewModel.Email;
                user.PasswordHash = PasswordHash.HashPassword(viewModel.ComparePassword);
                user.RegistrationDate = DateTime.UtcNow;
                
                var result = await _userManager.CreateAsync(user, user.PasswordHash);

                

                if (result != null)
                {
                    await _userManager.AddToRoleAsync(user, "Administrator");
                    Response.Cookies.Append("RegisteredUsername", user.UserName);
                    string logFile = Path.Combine(_webHostEnvironment.ContentRootPath, "Logs", "RegistrationLogs.txt");
                    using (StreamWriter sw = new StreamWriter(logFile, true))
                    {
                        sw.WriteLineAsync($"{viewModel.Name} зарегестрировался в {user.RegistrationDate}");
                        sw.Close();
                    }
                    return RedirectToAction("GreetingPage", "Home");
                }
                //var claims = new List<Claim>
                //{
                //    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                //    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.RoleType)
                //};

                ////ClaimsIdentity системный класс, который хранит все наши клаймы
                //ClaimsIdentity claimsIdentity = new ClaimsIdentity
                //(
                //    claims,
                //    "AppCookie", //в дальнейшем в HTTPContext можно будет найти под эти именем(можно называть как угодно).
                //    ClaimsIdentity.DefaultNameClaimType, //имя по умолчанию
                //    ClaimsIdentity.DefaultRoleClaimType //тип по умолчанию
                //);

                // Сохраним имя пользователя в TempData
                //TempData["RegisteredUsername"] = user.Name;
                // Или сохраним имя пользователя в куки
            }
            return View(viewModel);
        }
    }
}
