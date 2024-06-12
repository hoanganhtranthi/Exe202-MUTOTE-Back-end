

using MuTote.Domain.Commons;

namespace MuTote.Application.DTO.Response
{
    public class OrderResponse
    {
        public int Id { get; set; }
        public DateTime? OrderDate { get; set; }
        public decimal? TotalPrice { get; set; }
        [IntAttribute]
        public int? Status { get; set; }
        [IntAttribute]
        public int? CustomerId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public virtual CustomerResponse Customer { get; set; } = null!;
        public virtual ICollection<OrderDetailResponse> OrderDetails { get; set; }
    }
    public class OrderDetailResponse
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int OrderId { get; set; }
        public int? ProductId { get; set; }
        public string? Name { get; set; } = null!;
        public string? Img { get; set; }
        public decimal Price { get; set; }
    }
    public class OrderAmoutByCateRequest
    {
        public string CateName { get; set; }
        public int Total { get; set; }
        public decimal Amout { get; set; }
    }
}
