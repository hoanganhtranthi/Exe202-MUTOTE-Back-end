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
        public long? TotalPrice { get; set; }
        public bool? Status { get; set; }
        public int? CustomerId { get; set; }

        public virtual Customer? Customer { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
