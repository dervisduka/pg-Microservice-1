using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Filters;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ValuesController : ControllerBase
    {
        private readonly ICurrentActorService _user;

        public ValuesController(ICurrentActorService user)
        {
            this._user = user;

        }
        [HttpGet]
        [CustomPermission(typeof(Product), PermissionType.Access, new[] { AccessType.CanGet, AccessType.CanUpdate })]

        [CustomPermission(typeof(Product), PermissionType.Method, new[] { "Approve", "ApproveAdmin" })]
        public IActionResult Get() => Ok(new string[] { "value1", "value2" });
    }
}
