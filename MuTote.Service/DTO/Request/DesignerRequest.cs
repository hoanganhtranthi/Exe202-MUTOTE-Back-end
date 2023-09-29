using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuTote.Service.DTO.Request
{
    public class DesignerRequest
    {
        public string? Name { get; set; }
        public string? Email { get; set; } 
        public string? Phone { get; set; }
        public string? BankAccountNumber { get; set; }
        public int? Status { get; set; }
        public string? GoogleId { get; set; }
    }
}
