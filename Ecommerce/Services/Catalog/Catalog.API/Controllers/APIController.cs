using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [ApiVersion("V1")]
    [Route("api/v{version:ApiVersion}/[controller]")]
    [ApiController]
    public class APIController : ControllerBase
    {
    }
}
