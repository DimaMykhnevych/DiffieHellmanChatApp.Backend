using System.ComponentModel.DataAnnotations;

namespace ChatApp.Models
{
    public class AuthSignInModel
    {
        [Required]
        public string Username { get; set; }
    }
}
