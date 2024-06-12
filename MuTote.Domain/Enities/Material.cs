
namespace MuTote.Domain.Enities
{
    public partial class Material
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Img { get; set; } = null!;
        public int CategoryMaterialId { get; set; }
        public int? DesignerId { get; set; }

        public virtual CategoryMaterial CategoryMaterial { get; set; } = null!;
        public virtual Designer? Designer { get; set; }
    }
}
