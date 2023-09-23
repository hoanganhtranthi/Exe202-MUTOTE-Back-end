using System;
using System.Collections.Generic;

namespace MuTote.Data.Enities
{
    public partial class Material
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Img { get; set; } = null!;
        public int? CategoryMaterialId { get; set; }

        public virtual CategoryMaterial? CategoryMaterial { get; set; }
    }
}
