using AdvancedDevSample.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedDevSample.Domain.Pricing
{
    /// <summary>
    /// Contrat pour une stratégie de calcul de prix (promotion, remise, etc.).
    /// </summary>
    public interface IPricingStrategy
    {
        Price Calculate(Price basePrice);
    }
}

