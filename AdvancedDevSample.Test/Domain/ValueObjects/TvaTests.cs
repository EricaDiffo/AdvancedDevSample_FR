using AdvancedDevSample.Domain.Exceptions;
using AdvancedDevSample.Domain.ValueObjects;

namespace AdvancedDevSample.Test.Domain.ValueObjects
{
    public class TvaTests
    {
        [Fact]
        public void Constructor_Should_Throw_When_Rate_Is_Out_Of_Range()
        {
            Assert.Throws<DomainException>(() => new Tva(-0.01m));
            Assert.Throws<DomainException>(() => new Tva(1.01m));
        }

        [Fact]
        public void Apply_Should_Return_Price_With_Tva_Applied()
        {
            var basePrice = new Price(100m);
            var tva = new Tva(0.20m);

            var result = tva.Apply(basePrice);

            Assert.Equal(120m, result.Value);
        }
    }
}

