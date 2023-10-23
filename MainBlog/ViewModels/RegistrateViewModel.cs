using System.ComponentModel.DataAnnotations;

namespace MainBlog.ViewModels
{
    public class RegistrateViewModel
    {
        [DataType(DataType.Text)]
        [Display(Name = "Никнейм", Prompt = "Введите Никнейм")]
        [Required(ErrorMessage = "Поле 'Никнейм' обязательно для заполнения")]
        public string Name { get; set; }

        [Display(Name = "Возраст", Prompt = "Введите свой возраст")]
        public int Age { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Поле 'Почта' обязательно для заполнения")]
        [Display(Name = "Адрес Вашей электронной почты", Prompt = "Укажите свою почту")]
        public string Email { get; set; }

        [MinLength(4, ErrorMessage = "Длина пароля должна быть не меньше 3 символов")]
        [Required(ErrorMessage = "Поле 'Пароль' обязательно для заполнения")]
        [Display(Name = "Пароль", Prompt = "Введите пароль")]
        //[DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Обязательно подтвердите пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [Display(Name = "Подтвердить пароль", Prompt = "Введите пароль повторно")]
        public string ComparePassword { get; set; }
    }
}
