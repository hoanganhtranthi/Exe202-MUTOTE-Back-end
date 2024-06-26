﻿

using MuTote.Domain.Commons;
using System.ComponentModel.DataAnnotations;

namespace MuTote.Application.DTO.Response
{
   public class CustomerResponse
    {
        [Key]
        public int Id { get; set; }
        [StringAttribute]
        public string? Name { get; set; } = null!;
        public string? Avatar { get; set; }
        [StringAttribute]
        public string? Email { get; set; } = null!;
        [StringAttribute]
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public byte[] PasswordHash { get; set; } = null!;
        public byte[] PasswordSalt { get; set; } = null!;
        [StringAttribute]
        public string? Gender { get; set; }
        [StringAttribute]
        public string? GoogleId { get; set; }
        [IntAttribute]
        public int? Status { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}
