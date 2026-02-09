using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Application.Exceptions;
using AdvancedDevSample.Application.Services;
using AdvancedDevSample.Domain.Exceptions;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace AdvancedDevSample.Api.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Récupère la liste de tous les produits.
        /// </summary>
        /// <remarks>
        /// Cet endpoint retourne l'ensemble des produits du catalogue.
        /// Utiliser <c>/api/products/samples</c> pour voir des exemples de données.
        /// </remarks>
        /// <response code="200">Liste des produits trouvés.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProductResponse>), StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<ProductResponse>> GetAll()
        {
            var products = _productService.ListAll();
            return Ok(products);
        }

        /// <summary>
        /// Récupère le détail d'un produit par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant unique du produit.</param>
        /// <response code="200">Produit trouvé.</response>
        /// <response code="404">Produit introuvable.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ProductResponse> GetById(Guid id)
        {
            try
            {
                var product = _productService.GetById(id);
                return Ok(product);
            }
            catch (ApplicationServiceException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Modifie le prix d'un produit.
        /// </summary>
        /// <param name="id">Identifiant du produit.</param>
        /// <param name="request">Nouveau prix à appliquer.</param>
        /// <response code="204">Prix mis à jour.</response>
        /// <response code="400">Règle métier violée (prix invalide, produit inactif...).</response>
        /// <response code="404">Produit introuvable.</response>
        [HttpPut("{id}/price")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult ChangePrice(Guid id, [FromBody] ChangePriceRequest request)
        {
            try
            {
                _productService.ChangeProductPrice(id, request);
                return NoContent();
            }
            catch (ApplicationServiceException ex)
            {
                return NotFound(ex.Message);
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Applique une remise sur le prix du produit.
        /// </summary>
        /// <param name="id">Identifiant du produit.</param>
        /// <param name="discount">Montant de la remise à appliquer.</param>
        /// <response code="204">Remise appliquée.</response>
        /// <response code="400">Règle métier violée.</response>
        /// <response code="404">Produit introuvable.</response>
        [HttpPut("{id}/discount")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult ApplyDiscount(Guid id, [FromQuery] decimal discount)
        {
            try
            {
                _productService.ApplyDiscount(id, discount);
                return NoContent();
            }
            catch (ApplicationServiceException ex)
            {
                return NotFound(ex.Message);
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Active un produit.
        /// </summary>
        /// <param name="id">Identifiant du produit.</param>
        /// <response code="204">Produit activé.</response>
        /// <response code="404">Produit introuvable.</response>
        [HttpPut("{id}/activate")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Activate(Guid id)
        {
            try
            {
                _productService.ActivateProduct(id);
                return NoContent();
            }
            catch (ApplicationServiceException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Désactive un produit.
        /// </summary>
        /// <param name="id">Identifiant du produit.</param>
        /// <response code="204">Produit désactivé.</response>
        /// <response code="404">Produit introuvable.</response>
        [HttpPut("{id}/deactivate")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Deactivate(Guid id)
        {
            try
            {
                _productService.DeactivateProduct(id);
                return NoContent();
            }
            catch (ApplicationServiceException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Annuaire de jeux de données exemple pour les produits.
        /// </summary>
        /// <remarks>
        /// Cet endpoint retourne quelques produits de démonstration pour aider à comprendre la structure des données.
        /// Il sert de \"dictionnaire\" des jeux de données dans Swagger.
        /// </remarks>
        /// <response code="200">Exemples de produits retournés.</response>
        [HttpGet("samples")]
        [ProducesResponseType(typeof(IEnumerable<ProductResponse>), StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<ProductResponse>> GetSamples()
        {
            var samples = new List<ProductResponse>
            {
                new()
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Price = 9.99m,
                    IsActive = true
                },
                new()
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Price = 19.99m,
                    IsActive = false
                }
            };

            return Ok(samples);
        }
    }

}
