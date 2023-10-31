using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Tivoli.CustomerApi.Controllers;

[Controller]
[Route("[controller]")]
[Authorize]
public class ScannerController : ControllerBase
{
    
}