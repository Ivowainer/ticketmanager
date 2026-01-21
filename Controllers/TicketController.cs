using System.Data.Common;
using Microsoft.AspNetCore.Mvc;

namespace TicketManager.Controllers;

[ApiController]
[Route("[controller]")]
public class TicketController : ControllerBase
{
    [HttpGet(Name = "SS")]
    public IActionResult Get()
    {
        return NoContent();
    }
    
}