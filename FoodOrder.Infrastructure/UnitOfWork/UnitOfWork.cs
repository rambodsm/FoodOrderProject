using FoodOrder.Common.Result;
using FoodOrder.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace FoodOrder.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        public UnitOfWork(ApplicationDbContext context)
        {
            _dbContext = context;
        }
        public async Task<OperationResult> CommitAsync()
        {
            try
            {
                var result = await _dbContext.SaveChangesAsync();
                return OperationResult.BuildSuccessResult();
            }
            catch (Exception e)
            {
                //TODO:Log
                return OperationResult.BuildFailure(e, "خطا در هنگام ذخیره سازی");
            }

        }
    }
}
