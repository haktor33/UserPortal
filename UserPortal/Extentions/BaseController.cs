using Microsoft.AspNetCore.Mvc;

namespace UserPortal.Extensions
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
    }
}
