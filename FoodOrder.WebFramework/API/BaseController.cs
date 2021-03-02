using FoodOrder.Common.Enums;
using FoodOrder.WebFramework.Filters;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrder.WebFramework.API
{
    
    [ApiController]
    [ApiResultFilter]
    [Route("api/[controller]")]
    public class BaseController : ControllerBase
    {
        //public bool UserIsAutheticated => HttpContext.User.Identity.IsAuthenticated;
        //public string UserId => HttpContext.User.FindFirst(IdentityField.UserId.ToString()).Value;
    }
}
