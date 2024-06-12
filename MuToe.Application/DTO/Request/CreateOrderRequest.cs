
using System.ComponentModel.DataAnnotations;


namespace MuTote.Application.DTO.Request
{
    public class CreateOrderRequest
    {
        [Required]
        public int CustomerId { get; set; }
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Phone is invalid")]
        public string Phone { get; set; }
        public string Address { get; set; }
        public virtual ICollection<OrderDetailRequest> OrderDetails { get; set; }
    }
    public class OrderDetailRequest
    {
        public int Quantity { get; set; }
        public int ProductId { get; set; }
    }
}
