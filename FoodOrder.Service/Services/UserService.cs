using AutoMapper;
using AutoMapper.QueryableExtensions;
using FoodOrder.Common.Result;
using FoodOrder.Domain.Users;
using FoodOrder.Infrastructure.Contracts;
using FoodOrder.Presentation.Models.UserViewModels;
using FoodOrder.Service.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrder.Service.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        public UserService(IUserRepository userRepository, UserManager<User> userManager, IMapper mapper)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<OperationResult> CreateUserAsync(CreateUserViewModel model)
        {
            var result = await _userManager.CreateAsync(_mapper.Map<User>(model), model.Password);
            if (result.Succeeded)
                return OperationResult.BuildSuccessResult();
            return OperationResult.BuildFailure("خطا در هنگام ساختن کاربر");
        }
        public async Task<OperationResult<CreateUserViewModel>> GetUserByIdAsync(Guid userId)
        {
            var user = await _userRepository.TableNoTracking.Where(p => p.Id == userId)
                        .ProjectTo<CreateUserViewModel>(_mapper.ConfigurationProvider)
                        .SingleOrDefaultAsync();
            if (user is null)
                return OperationResult<CreateUserViewModel>.BuildFailure("کاربر مورد نظر یافت نشد");
            return OperationResult<CreateUserViewModel>.BuildSuccessResult(user);
        }
        public async Task<OperationResult<User>> CheckUserNameAndPasswordAsync(LoginUserViewModel model)
        {
            //TODO:CleanString UserName
            var user = await _userRepository.TableNoTracking.Where(p => p.UserName == model.UserName || p.Email == model.UserName).SingleOrDefaultAsync();
            if (user is null)
                return OperationResult<User>.BuildFailure("کاربر مورد نظر یافت نشد");
            var passwordValidation = await _userManager.CheckPasswordAsync(user, model.Password);
            if (passwordValidation is false)
                return OperationResult<User>.BuildFailure("نام کاربری یا رمز عبور اشتباه می باشد");
            return OperationResult<User>.BuildSuccessResult(user);
        }
    }
}
