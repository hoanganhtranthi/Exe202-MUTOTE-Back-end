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
        public string? Email { get; set; }
        public string? OldPassword { get; set; }
        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }
        [DataType(DataType.Password)]
        [Compare("NewPassword")]
        public string? ConfirmNewPassword { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; } = null;
        public string? Address { get; set; }
        public string? Avatar { get; set; }
    }
}
