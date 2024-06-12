
using MuTote.Domain.Commons;
using System.ComponentModel.DataAnnotations;


namespace MuTote.Application.DTO.Response
{
    public class CategoryResponse
    {
        [Key]
        public int Id { get; set; }
        [StringAttribute]
        public string Name { get; set; } = null!;
        public string? CateMaterialImg { get; set; }
        public string? CateProductImg { get; set; }
    }
}
