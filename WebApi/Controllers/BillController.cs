using Application.Common.Models;
using Application.DTO;
using Application.Entities.Bills.Commands;
using Application.Entities.Bills.Queries;
using Domain.Enums;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Filters;
using Application.Entities.Bills.Commands;
using Application.Entities.Bills.Queries;
using Asp.Versioning;
using Application.Entities.Bills.Commands;
using Application.Entities.Bills.Queries;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0", Deprecated = true)]
    [ApiVersion("2.0")]
    public class BillController : ApiControllerBase
    {
        private readonly ILogger<BillController> _logger;

        public BillController(ILogger<BillController> logger)
        {
            this._logger = logger;
        }

        [HttpGet]
        [MapToApiVersion("1.0")]

        public IActionResult GetAll()
        {
            //test info log in controller
            _logger.LogInformation("[Bill] V1 entered in getAll");
            try
            {
                int i = 0, j = 0;
                int k = i / j;
            }
            catch (Exception e)
            {
                //test catch log in controller
                _logger.LogError(e, "[Bill] V1 entered in catch");
            }

            var data = Mediator.Send(new GetAllBillsEmptyCriteria());

            return Ok(data);
        }
        /// <summary>
        /// This method gets all Bills and returns them as a list
        /// Version 2
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [MapToApiVersion("2.0")]

        public async Task<IActionResult> GetAll2()
        {
            //test info log in controller
            _logger.LogInformation("[Bill] V2 entered in getAll");
            try
            {
                int i = 0, j = 1;
                int k = i / j;
            }
            catch (Exception e)
            {
                //test catch log in controller
                _logger.LogError(e, "[Bill] V2 entered in catch");
            }

            var data = await Mediator.Send(new GetAllBillsEmptyCriteria());

            return Ok(data);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await Mediator.Send(new BillsWithPaginationCriteria { Id = id });

            if (data == null) return NotFound();
            if (data.Items.FirstOrDefault() == null) return NotFound();

            return Ok(data.Items.FirstOrDefault());
        }
        [HttpPost("[action]")]
        public async Task<ActionResult<Bill>> Create(CreateBillCommand command)
        {
            return await Mediator.Send(command);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {

            return Ok(await Mediator.Send(new DeleteBillCommand(id)));
        }
        [HttpPut]
        public async Task<IActionResult> Update(Bill prod)
        {

            return Ok(await Mediator.Send(new UpdateBillCommand(prod)));
        }

        [HttpGet("GetBillsWithPagination")]
        public async Task<ActionResult<PaginatedList<BillsBriefDto>>> GetBillsWithPagination([FromQuery] BillsWithPaginationCriteria query)
        {
            return await Mediator.Send(query);
        }

    }
}
