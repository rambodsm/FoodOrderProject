using FoodOrder.Infrastructure.Contracts;
using FoodOrder.Infrastructure.Persistance;

namespace FoodOrder.Infrastructure.Repositories.User
{
    public class UserRepository : Repository<Domain.Users.User>,IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}
