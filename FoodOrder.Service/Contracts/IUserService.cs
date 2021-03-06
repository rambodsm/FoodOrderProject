using FoodOrder.Common.Result;
using FoodOrder.Domain.Users;
using FoodOrder.Presentation.Models.UserViewModels;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrder.Service.Contracts
{
    public interface IUserService
    {
        Task<OperationResult<User>> CheckUserNameAndPasswordAsync(LoginUserViewModel model);
        Task<OperationResult> CreateUserAsync(CreateUserViewModel model);
        Task<OperationResult<AccessToken>> GetTokenAsync(LoginUserViewModel model);
        Task<User> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken);
        Task<OperationResult<CreateUserViewModel>> GetUserInformationByIdAsync(Guid userId);
        Task UpdateLastLoginDateAsync(User user);
    }
}
