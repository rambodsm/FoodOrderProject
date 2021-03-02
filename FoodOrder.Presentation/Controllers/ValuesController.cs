using FoodOrder.Domain.Users;
using FoodOrder.WebFramework.API;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrder.Presentation.Controllers
{
    public class ValuesController : BaseController
    {
        [HttpGet("Test1")]
        public IActionResult Test()
        {
            return Ok("I am here in test 1");
        }
        [HttpGet("Test2")]
        public ApiResult Test2()
        {
            return Ok();
        }
        [HttpGet("Test5")]
        public IActionResult Test5()
        {
            var user = new User();
            user.FirstName = "Rambod";
            return Ok(user);
        }
    }
}
