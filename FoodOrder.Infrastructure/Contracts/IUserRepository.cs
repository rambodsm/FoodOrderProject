using FoodOrder.Domain.Users;
using FoodOrder.Infrastructure.Repositories;

namespace FoodOrder.Infrastructure.Contracts
{
    public interface IUserRepository : IRepository<User>
    {
    }
}
