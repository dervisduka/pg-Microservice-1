using Application.Common.Models;
using Application.DTO;
using Application.Entities.Products.Commands;
using Application.Entities.Products.Queries;
using Asp.Versioning;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using WebApi.Filters;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0", Deprecated = true)]
    [ApiVersion("2.0")]
    public class ProductController : ApiControllerBase
    {

        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> logger)
        {
            this._logger = logger;
        }
        /// <summary>
        /// This method gets all products and returns them as a list
        /// Version 1
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [MapToApiVersion("1.0")]
        public IActionResult GetAll()
        {
            //test info log in controller
            _logger.LogInformation("[Product] V1 entered in getAll");
            try
            {
                int i = 0, j = 0;
                int k = i / j;
            }
            catch (Exception e)
            {
                //test catch log in controller
                _logger.LogError(e, "[Product] V1 entered in catch");
            }

            var data = Mediator.Send(new GetAllProductsEmptyCriteria());

            return Ok(data);
        }
        /// <summary>
        /// This method gets all products and returns them as a list
        /// Version 2
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [MapToApiVersion("2.0")]
        public async Task<IActionResult> GetAll2()
        {
            //test info log in controller
            _logger.LogInformation("[Product] V2 entered in getAll");
            try
            {
                int i = 0, j = 1;
                int k = i / j;
            }
            catch (Exception e)
            {
                //test catch log in controller
                _logger.LogError(e, "[Product] V2 entered in catch");
            }

            var data = await Mediator.Send(new GetAllProductsEmptyCriteria());

            return Ok(data);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await Mediator.Send(new ProductsWithPaginationCriteria { Id = id });

            if (data == null) return NotFound();
            if (data.Items.FirstOrDefault() == null) return NotFound();

            return Ok(data.Items.FirstOrDefault());
        }
        [HttpPost("[action]")]
        [CustomPermission(typeof(Product), PermissionType.Access, new[] { AccessType.CanGet, AccessType.CanUpdate })]
        [CustomPermission(typeof(Product), PermissionType.Method, new[] { "Metoda1", "Metoda2" })]
        [CustomPermission(typeof(Product), new[] { AccessType.CanGet, AccessType.CanUpdate }, new[] { "Metoda1", "Metoda2" })]
        [CustomPermission(typeof(Product), new[] { AccessType.CanGet, AccessType.CanUpdate })]
        [CustomPermission(typeof(Product), new[] { "Metoda1", "Metoda2" })]
        public async Task<ActionResult<Product>> Create(CreateProductCommand command)
        {
            return await Mediator.Send(command);
        }
        [HttpDelete]
        [CustomPermission(typeof(Product), new[] { AccessType.CanDelete })]
        public async Task<IActionResult> Delete(int id)
        {

            return Ok(await Mediator.Send(new DeleteProductCommand(id)));
        }
        [HttpPut]
        [CustomPermission(typeof(Product), new[] { AccessType.CanUpdate })]
        public async Task<IActionResult> Update(Product prod)
        {

            return Ok(await Mediator.Send(new UpdateProductCommand(prod)));
        }

        [HttpGet("GetProductsWithPagination")]
        [CustomPermission(typeof(Product), PermissionType.Access, new[] { AccessType.CanGet })]
        public async Task<ActionResult<PaginatedList<ProductsBriefDto>>> GetProductsWithPagination([FromQuery] ProductsWithPaginationCriteria query)
        {
            return await Mediator.Send(query);
        }


    }


}
