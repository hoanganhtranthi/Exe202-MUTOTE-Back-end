﻿using MuTote.Service.Commons;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuTote.Service.DTO.Request
{
    public class CreateDesignerRequest
    {
        [Required]
        [StringLength(int.MaxValue, MinimumLength = 3, ErrorMessage = "Invalid Name")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "Email is required !")]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [StringLength(11, MinimumLength = 10, ErrorMessage = "Phone is invalid")]
        public string? Phone { get; set; }
        [Required(ErrorMessage = "Password is required !")]
        [StringLength(int.MaxValue, MinimumLength = 1, ErrorMessage = "Password is invalid")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = " Confirm Password is required !")]
        [StringLength(int.MaxValue, MinimumLength = 1, ErrorMessage = "Confirm Password is invalid")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; } = null!;
        public string? Gender { get; set; }
        public string? GoogleId { get; set; }
        [DateRange]
        public DateTime? DateOfBirth { get; set; }
        [StringLength(14, MinimumLength = 9, ErrorMessage = "Invalid bank account number")]
        public string? BankAccountNumber { get; set; }
    }
}
