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
        [HttpGet]
        public ActionResult<IEnumerable<ProductResponse>> GetAll()
        {
            var products = _productService.ListAll();
            return Ok(products);
        }

        /// <summary>
        /// Récupère le détail d'un produit par son identifiant.
        /// </summary>
        [HttpGet("{id}")]
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

        [HttpPut("{id}/price")]
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
        [HttpPut("{id}/discount")]
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
        [HttpPut("{id}/activate")]
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
        [HttpPut("{id}/deactivate")]
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
    }

}
