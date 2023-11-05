using System.ComponentModel.DataAnnotations;

namespace MainBlog.ViewModels
{
    public enum RolesViewModel
    {
        [Display(Name = "User")]
        User,

        [Display(Name = "Moderator")]
        Moderator,

        [Display(Name = "Administrator")]
        Administrator
    }
}
