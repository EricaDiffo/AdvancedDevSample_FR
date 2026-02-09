using AdvancedDevSample.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedDevSample.Domain.Pricing
{
    /// <summary>
    /// Exemple de policy de pricing qui applique une stratégie donnée (ou aucune).
    /// </summary>
    public class CatalogPricingPolicy : IPricingPolicy
    {
        private readonly IPricingStrategy? _strategy;

        public CatalogPricingPolicy(IPricingStrategy? strategy = null)
        {
            _strategy = strategy;
        }

        public Price Apply(Price basePrice)
        {
            if (_strategy is null)
            {
                return basePrice;
            }

            return _strategy.Calculate(basePrice);
        }
    }
}

