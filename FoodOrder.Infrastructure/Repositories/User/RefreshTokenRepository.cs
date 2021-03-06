using FoodOrder.Domain.Users;
using FoodOrder.Infrastructure.Contracts;
using FoodOrder.Infrastructure.Persistance;

namespace FoodOrder.Infrastructure.Repositories.User
{
    public class RefreshTokenRepository : Repository<RefreshToken>,IRefreshTokenRepository
    {
        public RefreshTokenRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}
