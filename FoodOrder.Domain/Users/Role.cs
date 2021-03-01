using FoodOrder.Domain.Common;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodOrder.Domain.Users
{
    public class Role:IdentityRole<Guid>,IEntity
    {
        public Role()
        {
            Id = Guid.NewGuid();
        }
        public string Description { get; set; }
    }
}
