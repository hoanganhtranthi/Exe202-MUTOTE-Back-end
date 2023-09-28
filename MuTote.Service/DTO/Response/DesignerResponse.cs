using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuTote.Service.DTO.Response
{
    public class DesignerResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Avatar { get; set; }
        public string Email { get; set; } = null!;
        public byte[] PasswordHash { get; set; } = null!;
        public byte[] PasswordSalt { get; set; } = null!;
        public string? Phone { get; set; }
        public string? BankAccountNumber { get; set; }
    }
}
