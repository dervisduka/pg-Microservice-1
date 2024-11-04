using Application.Common.Models;
using Application.DTO;
using Application.Entities.Orders.Commands;
using Application.Entities.Orders.Queries;
using Application.Entities.Products.Commands;
using Asp.Versioning;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using WebApi.Filters;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ApiControllerBase
    {

        private readonly ILogger<OrderController> _logger;

        public OrderController(ILogger<OrderController> logger)
        {
            this._logger = logger;
        }
        /// <summary>
        /// This method gets all Orders and returns them as a list
        /// Version 1
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAll()
        {
            //test info log in controller
            _logger.LogInformation("[Order] V1 entered in getAll");
            try
            {
                int i = 0, j = 0;
                int k = i / j;
            }
            catch (Exception e)
            {
                //test catch log in controller
                _logger.LogError(e, "[Order] V1 entered in catch");
            }

            var data = Mediator.Send(new GetAllOrdersEmptyCriteria());

            return Ok(data.Result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await Mediator.Send(new OrdersWithPaginationCriteria { Id = id });

            if (data == null) return NotFound();
            if (data.Items.FirstOrDefault() == null) return NotFound();

            return Ok(data.Items.FirstOrDefault());
        }
        [HttpPost("[action]")]
        [CustomPermission(typeof(Order), PermissionType.Access, new[] { AccessType.CanGet, AccessType.CanUpdate })]
        [CustomPermission(typeof(Order), PermissionType.Method, new[] { "Metoda1", "Metoda2" })]
        [CustomPermission(typeof(Order), new[] { AccessType.CanGet, AccessType.CanUpdate }, new[] { "Metoda1", "Metoda2" })]
        [CustomPermission(typeof(Order), new[] { AccessType.CanGet, AccessType.CanUpdate })]
        [CustomPermission(typeof(Order), new[] { "Metoda1", "Metoda2" })]
        public async Task<ActionResult<Order>> Create(CreateOrderCommand command)
        {
            return await Mediator.Send(command);
        }
        [HttpDelete]
        [CustomPermission(typeof(Order), new[] { AccessType.CanDelete })]
        public async Task<IActionResult> Delete(int id)
        {

            return Ok(await Mediator.Send(new DeleteOrderCommand(id)));
        }
        [HttpPut]
        [CustomPermission(typeof(Order), new[] { AccessType.CanUpdate })]
        public async Task<IActionResult> Update(int id, [FromBody] OrdersUpdateDto command)
        {
            return Ok(await Mediator.Send(new UpdateOrderCommand(id, command)));
        }

        [HttpGet("GetOrdersWithPagination")]
        [CustomPermission(typeof(Order), PermissionType.Access, new[] { AccessType.CanGet })]
        public async Task<ActionResult<PaginatedList<OrdersBriefDto>>> GetOrdersWithPagination([FromQuery] OrdersWithPaginationCriteria query)
        {
            return await Mediator.Send(query);
        }


    }


}
