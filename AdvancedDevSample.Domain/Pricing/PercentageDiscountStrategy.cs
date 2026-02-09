using AdvancedDevSample.Domain.Exceptions;
using AdvancedDevSample.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedDevSample.Domain.Pricing
{
    /// <summary>
    /// Stratégie appliquant une remise en pourcentage (ex: 0.10 = -10%).
    /// </summary>
    public class PercentageDiscountStrategy : IPricingStrategy
    {
        public decimal Percentage { get; }

        public PercentageDiscountStrategy(decimal percentage)
        {
            if (percentage < 0 || percentage > 1)
            {
                throw new DomainException("Le pourcentage de remise doit être compris entre 0 et 1.");
            }

            Percentage = percentage;
        }

        public Price Calculate(Price basePrice)
        {
            var discounted = basePrice.Value * (1 - Percentage);
            return new Price(discounted);
        }
    }
}

