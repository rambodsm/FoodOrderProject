using AutoMapper;
using FoodOrder.Domain.Users;
using FoodOrder.Presentation.Models.UserViewModels;

namespace FoodOrder.Service.CustomMapping
{
    public class AutomapperProfileConfiguration : Profile
    {
        public AutomapperProfileConfiguration()
        {
            CreateMap<User, CreateUserViewModel>();
            CreateMap<CreateUserViewModel, User>();
        }
    }
}
