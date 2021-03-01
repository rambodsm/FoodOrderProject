using FoodOrder.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace FoodOrder.Domain.Restuarants
{
    public enum WeekDay
    {
        [Display(Name = "شنبه")]
        Saturday,
        [Display(Name = "یکشنبه")]
        Sunday,
        [Display(Name = "دوشنبه")]
        Monday,
        [Display(Name = "سه شنبه")]
        Tuesday,
        [Display(Name ="چهارشنبه")]
        Wednesday,
        [Display(Name = "پنج شنبه")]
        Thursday,
        [Display(Name = "جمعه")]
        Friday
    }
}
