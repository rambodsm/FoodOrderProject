using FoodOrder.Common.Result;
using FoodOrder.Domain.Users;
using System.Threading.Tasks;

namespace FoodOrder.Service.Contracts
{
    public interface IJwtService
    {
        Task<AccessToken> GenerateAsync(User user);
    }
}