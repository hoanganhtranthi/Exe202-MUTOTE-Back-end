using MuTote.Service.Commons;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuTote.Service.DTO.Request
{
    public class UpdateCustomerRequest
    {
        [Required]
        [StringLength(int.MaxValue, MinimumLength = 3, ErrorMessage = "Invalid Name")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "Email is required !")]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [StringLength(11, MinimumLength = 10, ErrorMessage = "Phone is invalid")]
        public string Phone { get; set; }
        public string? Address { get; set; }
        public string? OldPassword { get; set; }
        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }
        [DataType(DataType.Password)]
        [Compare("NewPassword")]
        public string? ConfirmNewPassword { get; set; }
        [StringLength(int.MaxValue, MinimumLength = 3, ErrorMessage = "Invalid Name")]
        public string? Gender { get; set; }
        [DateRange]
        public DateTime? DateOfBirth { get; set; }
        public string? Avatar { get; set; }
    }
}
