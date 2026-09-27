using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NotificationsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VersionController : Controller
    {
        [HttpGet]
        public string Get()
        {
            return "Version: 2";
        }
    }
}
