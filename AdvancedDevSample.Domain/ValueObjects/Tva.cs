using AdvancedDevSample.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedDevSample.Domain.ValueObjects
{
    /// <summary>
    /// Représente un taux de TVA (entre 0 et 1, ex: 0.20 pour 20%).
    /// </summary>
    public class Tva
    {
        /// <summary>
        /// Taux de TVA (0 <= Rate <= 1).
        /// </summary>
        public decimal Rate { get; init; }

        public Tva(decimal rate)
        {
            if (rate < 0 || rate > 1)
            {
                throw new DomainException("Le taux de TVA doit être compris entre 0 et 1.");
            }

            Rate = rate;
        }

        /// <summary>
        /// Calcule le prix TTC à partir d'un prix HT.
        /// </summary>
        public Price Apply(Price basePrice)
        {
            var ttc = basePrice.Value * (1 + Rate);
            return new Price(ttc);
        }

        public override string ToString() => $"{Rate:P0}";
    }
}

