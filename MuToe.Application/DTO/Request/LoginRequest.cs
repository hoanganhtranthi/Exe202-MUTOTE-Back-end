
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;


namespace MuTote.Application.DTO.Request
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Phone is required !")]
        [StringLength(11, MinimumLength = 10, ErrorMessage = "Phone is invalid")]
        public string Phone { get; set; }
       
        [Required(ErrorMessage = "Password is required !")]
        [PasswordPropertyText]
        public string Password { get; set; }
    }
}
