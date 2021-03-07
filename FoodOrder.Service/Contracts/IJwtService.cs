using FoodOrder.Common.Result;
using FoodOrder.Domain.Users;
using FoodOrder.Presentation.Models.UserViewModels;
using System.Threading.Tasks;

namespace FoodOrder.Service.Contracts
{
    public interface IJwtService
    {
        Task<AccessToken> GenerateAsync(User user);
        Task<OperationResult<AccessToken>> VerifyAndGenerateAsync(TokenRequestViewModel tokenRequest);
    }
}