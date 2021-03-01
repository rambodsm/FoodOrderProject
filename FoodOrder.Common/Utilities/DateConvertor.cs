using System;
using System.Globalization;

namespace FoodOrder.Common.Utilities
{
    public static class DateConvertor
    {
        public static string GetShamsiDate(this DateTime value)
        {
            PersianCalendar pc = new PersianCalendar();
            return pc.GetYear(value) + "/" + pc.GetMonth(value).ToString("00") + "/" + pc.GetDayOfMonth(value).ToString("00");
        }
        public static string GetShamsiTime(this DateTime value)
        {
            PersianCalendar pc = new PersianCalendar();
            return pc.GetHour(value).ToString("00") + ":" + pc.GetMinute(value).ToString("00");
        }
        public static string GetShamsiYearAndMonth(this DateTime value)
        {
            var pc = new PersianCalendar();
            return pc.GetYear(value) + "/" + pc.GetMonth(value).ToString("00");
        }
        public static string GetShamsiDateAndTime(this DateTime value)
        {
            PersianCalendar pc = new PersianCalendar();
            return pc.GetYear(value) + "/" + pc.GetMonth(value).ToString("00") + "/" +
                   pc.GetDayOfMonth(value).ToString("00") + " " + pc.GetHour(value).ToString("00") + ":" + pc.GetMinute(value).ToString("00");
        }
    }
}
