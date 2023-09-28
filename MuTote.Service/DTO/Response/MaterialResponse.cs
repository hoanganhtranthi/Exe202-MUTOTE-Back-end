using MuTote.Data.Enities;
using MuTote.Service.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuTote.Service.DTO.Response
{
    public class MaterialResponse
    {
        public int Id { get; set; }
        [StringAttribute]
        public string? Name { get; set; } = null!;
        public string Img { get; set; } = null!;
        [IntAttribute]
        public int? CategoryMaterialId { get; set; }

        public virtual CategoryResponse CategoryMaterial { get; set; }
    }
}
