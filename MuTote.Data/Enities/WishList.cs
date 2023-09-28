using System;
using System.Collections.Generic;

namespace MuTote.Data.Enities
{
    public partial class WishList
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }

        public virtual Customer Customer { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
    }
}
