using AutoMapper;
using AutoMapper.QueryableExtensions;
using FoodOrder.Common.Result;
using FoodOrder.Common.Utilities;
using FoodOrder.Domain.Users;
using FoodOrder.Infrastructure.Contracts;
using FoodOrder.Infrastructure.UnitOfWork;
using FoodOrder.Presentation.Models.UserViewModels;
using FoodOrder.Service.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrder.Service.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;
        private IJwtService _jwtService;
        private IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public UserService(IUserRepository userRepository, UserManager<User> userManager, IMapper mapper, IUnitOfWork uow, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _mapper = mapper;
            _uow = uow;
            _jwtService = jwtService;
        }
        public async Task<OperationResult> CreateUserAsync(CreateUserViewModel model)
        {
            var result = await _userManager.CreateAsync(_mapper.Map<User>(model), model.Password);
            if (result.Succeeded)
                return OperationResult.BuildSuccessResult();
            return OperationResult.BuildFailure("خطا در هنگام ساختن کاربر");
        }
        public async Task UpdateLastLoginDateAsync(User user)
        {
            user.LastLoginDate = DateTimeOffset.Now;
            _userRepository.Update(user);
            await _uow.CommitAsync();
        }
        public async Task<OperationResult<AccessToken>> GetTokenAsync(LoginUserViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user is null)
                return OperationResult<AccessToken>.BuildFailure("نام کاربری یا رمز عبور صحیح نمی باشد");
            var passwordValidation = await _userManager.CheckPasswordAsync(user, model.Password);
            if (passwordValidation is false)
                return OperationResult<AccessToken>.BuildFailure("نام کاربری یا رمز عبور صحیح نمی باشد");
            var token = await _jwtService.GenerateAsync(user);
            return OperationResult<AccessToken>.BuildSuccessResult(token);
        }
        public async Task<User> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _userRepository.GetByIdAsync(cancellationToken, userId);
        }
        public async Task<OperationResult<CreateUserViewModel>> GetUserInformationByIdAsync(Guid userId)
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
            model.UserName = model.UserName.CleanString();
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
