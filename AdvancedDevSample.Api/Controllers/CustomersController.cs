using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Application.Exceptions;
using AdvancedDevSample.Application.Services;
using AdvancedDevSample.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace AdvancedDevSample.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/customers")]
    public class CustomersController : ControllerBase
    {
        private readonly CustomerService _customerService;

        public CustomersController(CustomerService customerService)
        {
            _customerService = customerService;
        }

        /// <summary>
        /// Crée un nouveau client.
        /// </summary>
        /// <response code="201">Client créé.</response>
        [HttpPost]
        [ProducesResponseType(typeof(CustomerResponse), StatusCodes.Status201Created)]
        public ActionResult<CustomerResponse> Create([FromBody] CreateCustomerRequest request)
        {
            var customer = _customerService.Create(request);
            return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customer);
        }

        /// <summary>
        /// Récupère la liste de tous les clients.
        /// </summary>
        /// <response code="200">Liste des clients trouvés.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CustomerResponse>), StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<CustomerResponse>> GetAll()
        {
            var customers = _customerService.ListAll();
            return Ok(customers);
        }

        /// <summary>
        /// Récupère le détail d'un client par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant unique du client.</param>
        /// <response code="200">Client trouvé.</response>
        /// <response code="404">Client introuvable.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CustomerResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<CustomerResponse> GetById(Guid id)
        {
            try
            {
                var customer = _customerService.GetById(id);
                return Ok(customer);
            }
            catch (ApplicationServiceException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Active un client.
        /// </summary>
        /// <param name="id">Identifiant du client.</param>
        /// <response code="204">Client activé.</response>
        /// <response code="404">Client introuvable.</response>
        [HttpPut("{id}/activate")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Activate(Guid id)
        {
            try
            {
                _customerService.ActivateCustomer(id);
                return NoContent();
            }
            catch (ApplicationServiceException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Désactive un client.
        /// </summary>
        /// <param name="id">Identifiant du client.</param>
        /// <response code="204">Client désactivé.</response>
        /// <response code="404">Client introuvable.</response>
        [HttpPut("{id}/deactivate")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Deactivate(Guid id)
        {
            try
            {
                _customerService.DeactivateCustomer(id);
                return NoContent();
            }
            catch (ApplicationServiceException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Annuaire de jeux de données exemple pour les clients.
        /// </summary>
        /// <response code="200">Exemples de clients retournés.</response>
        [HttpGet("samples")]
        [ProducesResponseType(typeof(IEnumerable<CustomerResponse>), StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<CustomerResponse>> GetSamples()
        {
            return Ok(Api.Samples.CustomerSamples.All);
        }
    }
}

