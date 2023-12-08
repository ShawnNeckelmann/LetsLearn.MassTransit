using Microsoft.AspNetCore.Mvc;

namespace BurgerLink.Ui.Infrastructure;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseController : ControllerBase
{
}