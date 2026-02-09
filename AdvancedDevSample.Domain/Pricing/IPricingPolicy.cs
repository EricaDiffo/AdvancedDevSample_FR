using AdvancedDevSample.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedDevSample.Domain.Pricing
{
    /// <summary>
    /// Contrat pour une policy de pricing qui orchestre une ou plusieurs stratégies.
    /// </summary>
    public interface IPricingPolicy
    {
        /// <summary>
        /// Applique la politique de pricing à un prix de base.
        /// </summary>
        Price Apply(Price basePrice);
    }
}

