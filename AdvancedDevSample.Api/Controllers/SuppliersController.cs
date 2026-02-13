using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Application.Exceptions;
using AdvancedDevSample.Application.Interfaces;
using AdvancedDevSample.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace AdvancedDevSample.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/suppliers")]
    public class SuppliersController : ControllerBase
    {
        private readonly ISupplierService _supplierService;

        public SuppliersController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        /// <summary>
        /// Crée un nouveau fournisseur.
        /// </summary>
        /// <response code="201">Fournisseur créé.</response>
        /// <response code="400">Règle métier violée (nom vide, etc.).</response>
        [HttpPost]
        [ProducesResponseType(typeof(SupplierResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<SupplierResponse> Create([FromBody] CreateSupplierRequest request)
        {
            try
            {
                var supplier = _supplierService.Create(request);
                return CreatedAtAction(nameof(GetById), new { id = supplier.Id }, supplier);
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Récupère la liste de tous les fournisseurs.
        /// </summary>
        /// <response code="200">Liste des fournisseurs trouvés.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SupplierResponse>), StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<SupplierResponse>> GetAll()
        {
            var suppliers = _supplierService.ListAll();
            return Ok(suppliers);
        }

        /// <summary>
        /// Récupère le détail d'un fournisseur par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant unique du fournisseur.</param>
        /// <response code="200">Fournisseur trouvé.</response>
        /// <response code="404">Fournisseur introuvable.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SupplierResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<SupplierResponse> GetById(Guid id)
        {
            try
            {
                var supplier = _supplierService.GetById(id);
                return Ok(supplier);
            }
            catch (ApplicationServiceException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Met à jour les informations d'un fournisseur.
        /// </summary>
        /// <param name="id">Identifiant unique du fournisseur.</param>
        /// <param name="request">Nouvelles informations du fournisseur.</param>
        /// <response code="200">Fournisseur mis à jour.</response>
        /// <response code="400">Règle métier violée (nom vide, etc.).</response>
        /// <response code="404">Fournisseur introuvable.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(SupplierResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<SupplierResponse> Update(Guid id, [FromBody] UpdateSupplierRequest request)
        {
            try
            {
                var supplier = _supplierService.Update(id, request);
                return Ok(supplier);
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
        /// Supprime un fournisseur.
        /// </summary>
        /// <param name="id">Identifiant du fournisseur.</param>
        /// <response code="204">Fournisseur supprimé.</response>
        /// <response code="404">Fournisseur introuvable.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(Guid id)
        {
            try
            {
                _supplierService.Delete(id);
                return NoContent();
            }
            catch (ApplicationServiceException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
