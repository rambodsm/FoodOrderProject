using FoodOrder.Common.Result;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FoodOrder.Infrastructure.UnitOfWork
{
    public interface IUnitOfWork
    {
     
        Task<OperationResult> CommitAsync();
    }
}