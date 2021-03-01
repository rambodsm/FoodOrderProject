using System.ComponentModel.DataAnnotations;

namespace FoodOrder.Domain.Users
{
    public enum Gender
    {
        [Display(Name = "مرد")]
        Male = 1,

        [Display(Name = "زن")]
        Female = 2
    }
}
