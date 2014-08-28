using System.ComponentModel.DataAnnotations;

namespace GameStore.WebUI.Models
{
    public class LoginViewModel
    {
        [Required]
        public string userName { get; set; }
        [Required]
        public string password { get; set; }
    }
}