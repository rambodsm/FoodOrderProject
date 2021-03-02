using FoodOrder.WebFramework.Filters;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrder.WebFramework.API
{
    [ApiController]
    [ApiResultFilter]
    [Route("api/[controller]")]
    public class BaseController : ControllerBase
    {

    }
}
