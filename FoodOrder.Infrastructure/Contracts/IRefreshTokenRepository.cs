using FoodOrder.Domain.Users;
using FoodOrder.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodOrder.Infrastructure.Contracts
{
    public interface IRefreshTokenRepository:IRepository<RefreshToken>
    {

    }
}
