using System;
using System.Collections.Generic;

namespace MuTote.Data.Enities
{
    public partial class Customer
    {
        public Customer()
        {
            Orders = new HashSet<Order>();
            WishLists = new HashSet<WishList>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Avatar { get; set; }
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public byte[] PasswordHash { get; set; } = null!;
        public byte[] PasswordSalt { get; set; } = null!;
        public string? Gender { get; set; }
        public string? GoogleId { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<WishList> WishLists { get; set; }
    }
}
