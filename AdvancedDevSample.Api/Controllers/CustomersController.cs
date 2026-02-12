using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Application.Exceptions;
using AdvancedDevSample.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace AdvancedDevSample.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/customers")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IOrderService _orderService;

        public CustomersController(ICustomerService customerService, IOrderService orderService)
        {
            _customerService = customerService;
            _orderService = orderService;
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
        /// Met à jour les informations d'un client.
        /// </summary>
        /// <param name="id">Identifiant unique du client.</param>
        /// <param name="request">Nouvelles informations du client.</param>
        /// <response code="200">Client mis à jour.</response>
        /// <response code="404">Client introuvable.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(CustomerResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<CustomerResponse> Update(Guid id, [FromBody] UpdateCustomerRequest request)
        {
            try
            {
                var customer = _customerService.Update(id, request);
                return Ok(customer);
            }
            catch (ApplicationServiceException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Supprime un client.
        /// </summary>
        /// <param name="id">Identifiant du client.</param>
        /// <response code="204">Client supprimé.</response>
        /// <response code="400">Le client ne peut pas être supprimé (a des commandes existantes).</response>
        /// <response code="404">Client introuvable.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(Guid id)
        {
            try
            {
                _customerService.Delete(id);
                return NoContent();
            }
            catch (ApplicationServiceException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return NotFound(ex.Message);
            }
            catch (ApplicationServiceException ex) when (ex.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Récupère les commandes d'un client.
        /// </summary>
        /// <param name="id">Identifiant unique du client.</param>
        /// <response code="200">Liste des commandes du client.</response>
        /// <response code="404">Client introuvable.</response>
        [HttpGet("{id}/orders")]
        [ProducesResponseType(typeof(IEnumerable<OrderResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<OrderResponse>> GetOrders(Guid id)
        {
            try
            {
                // Vérifie que le client existe (et déclenche un 404 cohérent sinon)
                _customerService.GetById(id);

                var orders = _orderService
                    .ListAll()
                    .Where(o => o.CustomerId == id)
                    .ToList();

                return Ok(orders);
            }
            catch (ApplicationServiceException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // Endpoint /samples désactivé pour l'instant
    }
}

