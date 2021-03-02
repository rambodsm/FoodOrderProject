using FoodOrder.Infrastructure.Contracts;
using FoodOrder.Service.Contracts;

namespace FoodOrder.Service.Services
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
    }
}
