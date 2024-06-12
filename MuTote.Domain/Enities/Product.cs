
namespace MuTote.Domain.Enities
{
    public partial class Product
    {
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
            WishLists = new HashSet<WishList>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Img { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public int UnitInStock { get; set; }
        public int CategoryProductId { get; set; }
        public int? Status { get; set; }
        public bool? IsBestSeller { get; set; }

        public virtual CategoryProduct CategoryProduct { get; set; } = null!;
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<WishList> WishLists { get; set; }
    }
}
