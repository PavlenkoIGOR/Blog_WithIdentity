using System.ComponentModel.DataAnnotations;

namespace MainBlog.ViewModels
{
    public class LoginViewModel
    {
        [EmailAddress]
        [Display(Name = "Электронная почта", Prompt = "Введите свою электронную почту")]
        [Required(ErrorMessage = "Поле 'Никнейм' обязательно для заполнения")]
        public string? Email { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Поле 'Пароль' обязательно для заполнения")]
        [Display(Name = "Пароль", Prompt = "Введите пароль")]
        public string? Password { get; set; }

        [Display(Name = "Remember Me?")]
        public bool RememberMe;

        //public string? ReturnUrl { get; set; }
    }
}
