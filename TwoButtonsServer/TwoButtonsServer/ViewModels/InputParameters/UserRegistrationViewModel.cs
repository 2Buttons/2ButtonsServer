using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TwoButtonsServer.ViewModels.InputParameters
{
    public class UserRegistrationViewModel
    {
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
        //[Compare("Password", ErrorMessage = "Пароли не совпадают")]
        //public string ConfirmPassword { get; set; }
        public int Age { get; set; }
        public int Sex { get; set; }
        public string Phone { get; set; } = null;
        public string Description { get; set; } = null;
        public string FullAvatarLink { get; set; } = null;
        public string SmallAvatarLink { get; set; } = null;
    }
}
