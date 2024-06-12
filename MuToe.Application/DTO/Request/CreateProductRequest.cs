
using System.ComponentModel.DataAnnotations;


namespace MuTote.Application.DTO.Request
{
    public class CreateProductRequest
    {
        public string Name { get; set; } = null!;
        [Required]
        public string? Img { get; set; }
        [Range(10000, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        public decimal Price { get; set; }
        public string? Description { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        public int UnitInStock { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        public int CategoryProductId { get; set; }
    }
}
