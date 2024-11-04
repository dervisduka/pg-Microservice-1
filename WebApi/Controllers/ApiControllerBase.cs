using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;



namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] po e le koment per shembull
    public abstract class ApiControllerBase : ControllerBase
    {
        private ISender _mediator = null!;
        private string _username = null!;
        private string _userId = null!;

        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
        protected string UserName => _username ??= HttpContext.User.FindFirstValue(ClaimTypes.Name);
        public string UserId => _userId ??= HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
