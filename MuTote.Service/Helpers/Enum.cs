using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuTote.Service.Helpers
{
    public class Enum
    {      
        public enum ProductStatusEnum
        {
            NewProduct=1,
            Avaliable = 2,
            OutOfStock = 0
        }
        public enum OrderStatusEnum
        {          
            Pending = 0,
            Finish = 1
        }
        public enum PaymentEnum
        {
            Pending = 1,
            Finish = 0
        }
        public enum CategoryChoice
        {
            Product=1,
            Material=0
        }
    }
}
