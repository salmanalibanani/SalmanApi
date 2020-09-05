using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalmanApi
{
    public class ShopperHistory
    {
        public int customerId { get; set; }
        public Product[] products { get; set; }
    }
}
