
using System.ComponentModel.DataAnnotations;


namespace MuTote.Application.DTO.Request
{
    public class CreateCategoryRequest
    {
        [Required]
        public string Name { get; set; } = null!;
        public string? CateMaterialImg { get; set; }
        public string? CateProductImg { get; set; }
    }
}