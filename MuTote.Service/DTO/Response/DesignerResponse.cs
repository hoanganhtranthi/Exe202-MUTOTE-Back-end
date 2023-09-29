using MuTote.Service.Commons;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuTote.Service.DTO.Response
{
    public class DesignerResponse
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
        public byte[] PasswordHash { get; set; } = null!;
        public byte[] PasswordSalt { get; set; } = null!;
        [StringAttribute]
        public string? Gender { get; set; }
        [StringAttribute]
        public string? GoogleId { get; set; }
        [IntAttribute]
        public int? Status { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Token { get; set; }
        [StringAttribute]
        public string? BankAccountNumber { get; set; }
    }
}
