using System.ComponentModel.DataAnnotations;

namespace FoodOrder.Domain.Orders
{
    public enum OrderStatus
    {
        [Display(Name = "باز")]
        Open,
        [Display(Name = "در حال انجام")]
        InProgress,
        [Display(Name = "بسته")]
        Close
    }
}
