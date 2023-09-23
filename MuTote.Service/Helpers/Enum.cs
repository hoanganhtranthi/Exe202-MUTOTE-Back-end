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
            Avaliable = 1,
            OutOfStock = 0
        }
        public enum OrderStatusEnum
        {
            StoreCancel = 0,
            Pending = 1,
            Assign = 2,
            Finish = 3
        }
        public enum PaymentEnum
        {
            Pending = 1,
            Finish = 0
        }
    }
}
