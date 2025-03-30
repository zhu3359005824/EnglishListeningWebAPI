using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IDentity.WebAPI.Controllers.Login
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize]

    public class AdminController : ControllerBase
    {

    }
}
