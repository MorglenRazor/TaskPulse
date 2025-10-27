using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskPulse.Core.Services
{
    public class PricingService
    {
        public bool IsValidPrice(decimal price) => price > 0 && price < 10000;
    }
}
