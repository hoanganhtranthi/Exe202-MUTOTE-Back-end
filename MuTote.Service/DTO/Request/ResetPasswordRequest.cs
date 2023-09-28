﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuTote.Service.DTO.Request
{
    public class ResetPasswordRequest
    {
        [Required(ErrorMessage = "Phone is required !")]
        public string Phone { get; set; }
        [Required(ErrorMessage = " New Password is required !")]
        [StringLength(int.MaxValue, MinimumLength = 1, ErrorMessage = "New Password is invalid")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = " Confirm Password is required !")]
        [StringLength(int.MaxValue, MinimumLength = 1, ErrorMessage = " Confirm New Password is invalid")]
        [DataType(DataType.Password)]
        [Compare("NewPassword")]
        public string ConfirmNewPassword { get; set; } = null!;
    }
}
