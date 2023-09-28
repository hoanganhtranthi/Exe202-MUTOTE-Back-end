using System;
using System.Collections.Generic;

namespace MuTote.Data.Enities
{
    public partial class CategoryProduct
    {
        public CategoryProduct()
        {
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? CateProductImg { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
