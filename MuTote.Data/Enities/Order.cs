using System;
using System.Collections.Generic;

namespace MuTote.Data.Enities
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }
        public DateTime? OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public int Status { get; set; }
        public int CustomerId { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        public virtual Customer Customer { get; set; } = null!;
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
