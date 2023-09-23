using System;
using System.Collections.Generic;

namespace MuTote.Data.Enities
{
    public partial class Designer
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Avatar { get; set; }
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public string? BankAccountNumber { get; set; }
        public byte[] PasswordHash { get; set; } = null!;
        public byte[] PasswordSalt { get; set; } = null!;
        public string? GoogleId { get; set; }
        public string? Gender { get; set; }
    }
}
