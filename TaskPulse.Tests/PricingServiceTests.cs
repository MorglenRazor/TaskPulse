using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using TaskPulse.Core.Services;

namespace TaskPulse.Tests
{
    public class PricingServiceTests
    {
        [Theory]
        [InlineData(50)]
        [InlineData(9999)]
        public void ValidPrices_ShouldReturnTrue(decimal price)
        {
            var service = new PricingService();
            service.IsValidPrice(price).Should().BeTrue();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        [InlineData(10001)]
        public void InvalidPrices_ShouldReturnFalse(decimal price)
        {
            var service = new PricingService();
            service.IsValidPrice(price).Should().BeFalse();
        }
    }
}
