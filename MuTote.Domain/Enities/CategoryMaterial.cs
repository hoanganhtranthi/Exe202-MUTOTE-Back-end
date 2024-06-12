

namespace MuTote.Domain.Enities
{
    public partial class CategoryMaterial
    {
        public CategoryMaterial()
        {
            Materials = new HashSet<Material>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? CateMaterialImg { get; set; }

        public virtual ICollection<Material> Materials { get; set; }
    }
}
