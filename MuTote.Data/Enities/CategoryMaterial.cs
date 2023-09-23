using System;
using System.Collections.Generic;

namespace MuTote.Data.Enities
{
    public partial class CategoryMaterial
    {
        public CategoryMaterial()
        {
            Materials = new HashSet<Material>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Material> Materials { get; set; }
    }
}
