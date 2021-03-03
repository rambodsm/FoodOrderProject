﻿using FoodOrder.Domain.Users;
using System.ComponentModel.DataAnnotations;

namespace FoodOrder.Presentation.Models.UserViewModels
{
    public class CreateUserViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public Gender Gender { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class LoginUserViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
