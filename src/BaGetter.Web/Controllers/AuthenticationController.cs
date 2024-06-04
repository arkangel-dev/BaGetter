using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaGetter.Web.Controllers;

[Route("[controller]")]
public class AuthenticationController : ControllerBase
{

    [HttpGet("auth")]
    public IActionResult Get()
    {
        return Ok("Hello world!");
    }
}
