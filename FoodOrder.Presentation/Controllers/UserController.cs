using FoodOrder.Presentation.Models.UserViewModels;
using FoodOrder.Service.Contracts;
using FoodOrder.WebFramework.API;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
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
        [HttpPost]
        [SwaggerOperation(Summary ="Create User",Description = "Gender number 1 is Male and Gender number 2 is Female")]
        [SwaggerResponse(200,"Create User was Success")]
        [SwaggerResponse(400,"Create User was failed,Beacuse UserInput or DataBase Error or..")]
        public async Task<IActionResult> Create(CreateUserViewModel user)
        {
            if (ModelState.IsValid is false)
                return BadRequest("مقادیر وارد شده معتبر نمی باشد");
            var result = await _service.CreateUserAsync(user);
            if (result.Success is false)
                return BadRequest(result.ErrorMessage);
            return Ok();
        }
        [HttpGet("{userId:Guid}")]
        [SwaggerOperation(Summary = "Get User")]
        [SwaggerResponse(200, "Operation was success")]
        [SwaggerResponse(400, "UserId Was wrong")]
        public async Task<IActionResult> GetUserById(Guid userId)
        {
            if (userId == Guid.Empty)
                return BadRequest("آیدی وارد شده صحیح نمی باشد");
            var result = await _service.GetUserInformationByIdAsync(userId);
            if (result.Success is false)
                return BadRequest("کاربر مورد نظر یافت نشد");
            return Ok(result.Result);
        }
        [HttpPost("[action]")]
        [SwaggerOperation(Summary ="Get Access And Refresh Token",Description ="Username can be Email,UserName,Phone")]
        [SwaggerResponse(200, "Operation was success")]
        [SwaggerResponse(400, "UserName or Password was wrong")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginUserViewModel model)
        {
            var result = await _service.GetTokenAsync(model);
            if (result.Success is false)
                return BadRequest(result.ErrorMessage);
            return Ok(result.Result);
        }
    }
}
