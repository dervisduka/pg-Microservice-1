using Application.Common.Models;
using Application.DTO;
using Application.Entities.Alcohols.Commands;
using Application.Entities.Alcohols.Queries;
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
    public class AlcoholController : ApiControllerBase
    {

        private readonly ILogger<AlcoholController> _logger;

        public AlcoholController(ILogger<AlcoholController> logger)
        {
            this._logger = logger;
        }
        /// <summary>
        /// This method gets all Alcohols and returns them as a list
        /// Version 1
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAll()
        {
            //test info log in controller
            _logger.LogInformation("[Alcohol] V1 entered in getAll");
            try
            {
                int i = 0, j = 1;
                int k = i / j;
            }
            catch (Exception e)
            {
                //test catch log in controller
                _logger.LogError(e, "[Alcohol] V1 entered in catch");
            }

            var data = Mediator.Send(new GetAllAlcoholsEmptyCriteria());

            return Ok(data.Result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await Mediator.Send(new AlcoholsWithPaginationCriteria { Id = id });

            if (data == null) return NotFound();
            if (data.Items.FirstOrDefault() == null) return NotFound();

            return Ok(data.Items.FirstOrDefault());
        }
        [HttpPost("[action]")]
       public async Task<ActionResult<Alcohol>> Create(CreateAlcoholCommand command)
        {
            return await Mediator.Send(command);
        }
        [HttpDelete]
         public async Task<IActionResult> Delete(int id)
        {

            return Ok(await Mediator.Send(new DeleteAlcoholCommand(id)));
        }
        [HttpPut]
      public async Task<IActionResult> Update(int id, [FromBody] AlcoholUpdateDto command)
        {
            return Ok(await Mediator.Send(new UpdateAlcoholCommand(id, command)));
        }

        [HttpGet("GetAlcoholsWithPagination")]
        [CustomPermission(typeof(Alcohol), PermissionType.Access, new[] { AccessType.CanGet })]
        public async Task<ActionResult<PaginatedList<AlcoholBriefDto>>> GetAlcoholsWithPagination([FromQuery] AlcoholsWithPaginationCriteria query)
        {
            return await Mediator.Send(query);
        }


    }


}
