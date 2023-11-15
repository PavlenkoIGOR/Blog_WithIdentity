using MainBlog.BL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;
using MainBlog.ViewModels;
using MainBlog.Data.Models;
using Microsoft.AspNetCore.Hosting;

namespace MainBlog.Controllers
{

    public class AuthRegController : Controller
    {
        ILogger _logger;

		private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private IWebHostEnvironment _env;
        public AuthRegController(UserManager<User> userManager, SignInManager<User> signInManager, IWebHostEnvironment environment, ILogger logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _env = environment;
            _logger = logger;
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
                        await _userManager.AddToRoleAsync(user, roles[2]);
                        await _signInManager.SignInAsync(user, false);
                        await WriteActions.CreateLogFolder_File(_env, "RegistrationLogs", $"зарегестрировался пользователь {viewModel.Name}");
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
        public IActionResult Login(string rtrnUrl)
        {
            return View(new LoginViewModel { ReturnUrl = rtrnUrl });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {

            string filePath22 = Path.Combine(_env.ContentRootPath, "Logs", "LoginStateLogs.txt");
            if (ModelState.IsValid)
            {
                foreach (var elem in ModelState)
                {
                    var k = elem.Key;
                    var v = elem.Value;
                }
                await WriteActions.CreateLogFolder_File(_env, "LoginModelState", $"Модель при входе {model.Email} валидна"); //Ы-ы-ы-ы :)

                User user = await _userManager.FindByEmailAsync(model.Email);

                if (user == null)
                {
                    await WriteActions.CreateLogFolder_File(_env, "LoginLogs", $"Неудачная попытка залогиниться! Почта: {model.Email}, пароль {model.Password}"); //Ы-ы-ы-ы :)

                    return StatusCode(400);
				}
                await _signInManager.SignOutAsync(); // аннулирует любой имеющийся у пользователя сеанс????
                SignInResult signInResult = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);//проводит уже саму аутентификацию. Второй false - должна ли учётка блокироваться в случае некорректного пароля
                if (signInResult.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    await WriteActions.CreateLogFolder_File(_env, "LoginLogs", $"Выполнен вход! Почта: {model.Email}, пароль {model.Password}"); //Ы-ы-ы-ы :)
                    return RedirectToAction("GreetingPage", "Home");
                }
                else 
                {
                    return StatusCode(400);
                }
            }
            ////это не нужно, так как за пустые поля отвечает script на странице логин
            //if (String.IsNullOrEmpty(model.Email) || String.IsNullOrEmpty(model.Password))
            //    throw new ArgumentNullException(model.Email);
            //тоже не нужно, не отработает, т.к. есть проверка на null
            //if (!ModelState.IsValid)
            //{
            //    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
            //}
            await WriteActions.CreateLogFolder_File(_env, "LoginLogs", $"ModelState.IsValid = false!: {model.Email}, пароль {model.Password}"); //Ы-ы-ы-ы :)
            return StatusCode(404);
        }
        #endregion
        #region Logout

        [HttpPost]
        [ValidateAntiForgeryToken] //ValidateAntiForgeryToken будет работать только, с HttpPost, причем в cshtml надо указать именно <form method="post">
        public async Task<IActionResult> Logout()
        {
            User a = await _signInManager.UserManager.GetUserAsync(User);
            await WriteActions.CreateLogFolder_File(_env, "LogOutLogs", $"выпилился {a.Email}"); //Ы-ы-ы-ы :)
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
        #endregion

    }
}


