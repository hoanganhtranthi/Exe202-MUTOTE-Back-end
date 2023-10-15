using MuTote.Data.Enities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuTote.Service.DTO.Request
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
