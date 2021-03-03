using FoodOrder.Common.Result;
using FoodOrder.Domain.Users;
using FoodOrder.Presentation.Models.UserViewModels;
using System;
using System.Threading.Tasks;

namespace FoodOrder.Service.Contracts
{
    public interface IUserService
    {
        Task<OperationResult<User>> CheckUserNameAndPasswordAsync(LoginUserViewModel model);
        Task<OperationResult> CreateUserAsync(CreateUserViewModel model)
Task<OperationResult<CreateUserViewModel>> GetUserByIdAsync(Guid userId);
    }
}
