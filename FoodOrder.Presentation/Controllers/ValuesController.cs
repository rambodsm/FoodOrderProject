using FoodOrder.Domain.Users;
using FoodOrder.Service.Contracts;
using FoodOrder.WebFramework.API;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrder.Presentation.Controllers
{
    [Authorize]
    public class ValuesController : BaseController
    {
        private IUserService _userService { get; set; }
        public ValuesController(IUserService userService)
        {
            _userService = userService;
        }

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
        [AllowAnonymous]
        public IActionResult Test5()
        {
            var user = new User();
            user.FirstName = "Rambod";
            return Ok(user);
        }
    }
}
