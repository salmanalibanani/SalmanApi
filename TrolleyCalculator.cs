using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalmanApi
{
    public class TrolleyCalculator
    {
        private TrolleyInfo _trolleyInfo;
        private decimal _trolleyCost;

        public TrolleyCalculator(TrolleyInfo trolleyInfo)
        {
            _trolleyInfo = trolleyInfo;
            _trolleyCost = 0;
        }

        public decimal Calculate()
        {
            return 5;
        }

        public decimal GetPrice(string productName)
        {
            return _trolleyInfo.products.Where(x => x.name.Equals(productName)).FirstOrDefault().price;
        }

        public int GetCompleteSpecialsCount(Special special)
        {
            // Will find complete special sets.  Leave the remaining quantities in the "quantities" list

            int completeSpecialsCount = 0;

            foreach (var specialProduct in special.quantities)
            {
                var p = _trolleyInfo.quantities.Where(x => x.name.Equals(specialProduct.name)).FirstOrDefault();
                if (p == null) break;

                if (p.quantity >= specialProduct.quantity)
                {

                }
                foreach (var product in _trolleyInfo.quantities)
                {

                }
            }

            var pqrst = _trolleyInfo.quantities;

            return 10;
        }
    }
}
