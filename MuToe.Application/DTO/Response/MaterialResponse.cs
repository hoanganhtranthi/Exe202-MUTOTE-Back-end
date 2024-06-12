

using MuTote.Domain.Commons;
using System.ComponentModel.DataAnnotations;

namespace MuTote.Application.DTO.Response
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
