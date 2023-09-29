using MuTote.Data.Enities;
using MuTote.Service.Commons;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuTote.Service.DTO.Response
{
    public class MaterialResponse
    {
        [Key]
        public int Id { get; set; }
        [StringAttribute]
        public string? Name { get; set; } = null!;
        public string Img { get; set; } = null!;
        [IntAttribute]
        public int? CategoryMaterialId { get; set; }
        [IntAttribute]
        public int? DesignerId { get; set; }
        public virtual DesignerResponse? Designer { get; set; }
        public virtual CategoryResponse CategoryMaterial { get; set; }
    }
}
