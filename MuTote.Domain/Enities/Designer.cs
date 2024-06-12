

namespace MuTote.Domain.Enities
{
    public partial class Designer
    {
        public Designer()
        {
            Materials = new HashSet<Material>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Avatar { get; set; }
        public string Email { get; set; } = null!;
        public byte[] PasswordHash { get; set; } = null!;
        public byte[] PasswordSalt { get; set; } = null!;
        public string? Phone { get; set; }
        public string? BankAccountNumber { get; set; }
        public int Status { get; set; }
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? GoogleId { get; set; }

        public virtual ICollection<Material> Materials { get; set; }
    }
}
