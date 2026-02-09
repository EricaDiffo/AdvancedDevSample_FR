using AdvancedDevSample.Domain.Pricing;
using AdvancedDevSample.Domain.ValueObjects;

namespace AdvancedDevSample.Test.Domain.Pricing
{
    public class PricingTests
    {
        [Fact]
        public void PercentageDiscountStrategy_Should_Apply_Discount()
        {
            var basePrice = new Price(100m);
            var strategy = new PercentageDiscountStrategy(0.10m); // -10%

            var result = strategy.Calculate(basePrice);

            Assert.Equal(90m, result.Value);
        }

        [Fact]
        public void CatalogPricingPolicy_Should_Return_BasePrice_When_No_Strategy()
        {
            var basePrice = new Price(100m);
            var policy = new CatalogPricingPolicy();

            var result = policy.Apply(basePrice);

            Assert.Equal(100m, result.Value);
        }

        [Fact]
        public void CatalogPricingPolicy_Should_Use_Strategy_When_Provided()
        {
            var basePrice = new Price(200m);
            var strategy = new PercentageDiscountStrategy(0.25m); // -25%
            var policy = new CatalogPricingPolicy(strategy);

            var result = policy.Apply(basePrice);

            Assert.Equal(150m, result.Value);
        }
    }
}

