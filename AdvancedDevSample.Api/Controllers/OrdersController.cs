using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Application.Exceptions;
using AdvancedDevSample.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace AdvancedDevSample.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Crée une nouvelle commande.
        /// </summary>
        /// <response code="201">Commande créée.</response>
        [HttpPost]
        [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status201Created)]
        public ActionResult<OrderResponse> Create([FromBody] CreateOrderRequest request)
        {
            var order = _orderService.Create(request);
            return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
        }

        /// <summary>
        /// Récupère la liste de toutes les commandes.
        /// </summary>
        /// <response code="200">Liste des commandes trouvées.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<OrderResponse>), StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<OrderResponse>> GetAll()
        {
            var orders = _orderService.ListAll();
            return Ok(orders);
        }

        /// <summary>
        /// Récupère le détail d'une commande par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant unique de la commande.</param>
        /// <response code="200">Commande trouvée.</response>
        /// <response code="404">Commande introuvable.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<OrderResponse> GetById(Guid id)
        {
            try
            {
                var order = _orderService.GetById(id);
                return Ok(order);
            }
            catch (ApplicationServiceException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Récupère les lignes (produits) d'une commande donnée.
        /// </summary>
        /// <param name="id">Identifiant unique de la commande.</param>
        /// <response code="200">Liste des produits de la commande.</response>
        /// <response code="404">Commande introuvable.</response>
        [HttpGet("{id}/items")]
        [ProducesResponseType(typeof(IEnumerable<OrderItemResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<OrderItemResponse>> GetItems(Guid id)
        {
            try
            {
                var order = _orderService.GetById(id);
                return Ok(order.Items);
            }
            catch (ApplicationServiceException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Annuaire de jeux de données exemple pour les commandes.
        /// </summary>
        /// <response code="200">Exemples de commandes retournées.</response>
        //[HttpGet("samples")]
       /// [ProducesResponseType(typeof(IEnumerable<OrderResponse>), StatusCodes.Status200OK)]
        ///public ActionResult<IEnumerable<OrderResponse>> GetSamples()
        ///{
           /// return Ok(Api.Samples.OrderSamples.All);
        ///}
    }
}

