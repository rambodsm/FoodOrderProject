using FoodOrder.Presentation.Models.UserViewModels;
using FoodOrder.Service.Contracts;
using FoodOrder.WebFramework.API;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrder.Presentation.Controllers
{
    public class UserController : BaseController
    {
        private IUserService _service;
        public UserController(IUserService service)
        {
            _service = service;
        }
        [HttpPost("{UserViewModel}")]
        public async Task<IActionResult> CreateUserAsync(CreateUserViewModel user)
        {
            if (ModelState.IsValid is false)
                return BadRequest("مقادیر وارد شده معتبر نمی باشد");
            var result = await _service.CreateUserAsync(user);
            if (result.Success is false)
                return BadRequest(result.ErrorMessage);
            return Ok();
        }
        [HttpGet("{userId:Guid}")]
        public async Task<IActionResult> GetUserById(Guid userId)
        {
            var result = await _service.GetUserByIdAsync(userId);
            if (result.Success is false)
                return BadRequest("کاربر مورد نظر یافت نشد");
            return Ok(result);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> GetToken(LoginUserViewModel model)
        {
            var result = await _service.CheckUserNameAndPasswordAsync(model);
            if (result.Success is false)
                return BadRequest(result.ErrorMessage);
            //TODO:Get Jwt Token
            return Ok();
        }
    }
}
