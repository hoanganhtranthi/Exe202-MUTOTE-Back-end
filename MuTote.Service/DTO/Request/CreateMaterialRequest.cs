using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuTote.Service.DTO.Request
{
    public class CreateMaterialRequest
    {
        public string Name { get; set; } = null!;
        public string Img { get; set; } = null!;
        public int CategoryMaterialId { get; set; }
    }
}
