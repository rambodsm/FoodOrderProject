using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace FoodOrder.Common.Utilities
{
    public static class EnumExtensions
    {
        public static IEnumerable<T> GetEnumValues<T>(this T input) where T : struct
        {
            if (!typeof(T).IsEnum)
                throw new NotSupportedException();

            return Enum.GetValues(input.GetType()).Cast<T>();
        }

        public static IEnumerable<T> GetEnumFlags<T>(this T input) where T : struct
        {
            if (!typeof(T).IsEnum)
                throw new NotSupportedException();

            foreach (var value in Enum.GetValues(input.GetType()))
                if ((input as Enum).HasFlag(value as Enum))
                    yield return (T)value;
        }
        public static TAttribute GetAttribute<TAttribute>(this Enum enumValue) where TAttribute : Attribute
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<TAttribute>();
        }
        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static SelectList ToSelectList(this Type enumType, SelectListOptions options = null)
        {
            if (options == null)
                options = new SelectListOptions();

            var selectList = new List<SelectListItem>();

            var categories = enumType.GetCategories(options.DisabledGroups);

            if (options.Placeholder != null)
                selectList.Add(new SelectListItem(options.Placeholder, ""));

            var values = Enum.GetValues(enumType);
            foreach (var value in values)
            {
                bool selected = (options.SelectedValues.Any(v => v == value));
                bool disabled = (options.DisabledValues.Any(v => v == value));
                var group = categories.FirstOrDefault(c => c.Name == ((Enum)value).GetCategoryName());
                string listValues = options.IsStringValue ? ((Enum)value).ToString() : ((int)value).ToString();

                var item = new SelectListItem(((Enum)value).GetDisplayName(), listValues, selected, disabled);
                item.Group = group;
                selectList.Add(item);
            }

            return new SelectList(selectList);
        }

        public static string GetDisplayName(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DisplayNameAttribute[] attributes = fi.GetCustomAttributes(typeof(DisplayNameAttribute), false) as DisplayNameAttribute[];

            if (attributes != null && attributes.Any())
            {
                return attributes.First().DisplayName;
            }

            return value.ToString();
        }

        public static string GetCategoryName(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            CategoryAttribute[] attributes = fi.GetCustomAttributes(typeof(CategoryAttribute), false) as CategoryAttribute[];

            if (attributes != null && attributes.Any())
            {
                return attributes.First().Category;
            }

            return "";
        }

        private static IEnumerable<SelectListGroup> GetCategories(this Type enumType, string[] DisabledGroups)
        {
            var categories = new List<SelectListGroup>();
            var fields = enumType.GetFields();

            foreach (var fi in fields)
            {
                CategoryAttribute[] attributes = fi.GetCustomAttributes(typeof(CategoryAttribute), false) as CategoryAttribute[];
                if (attributes != null && attributes.Any())
                {
                    string categoryName = attributes.First().Category;
                    bool exists = categories.Any(c => c.Name == categoryName);
                    if (!exists)
                    {
                        bool disabled = DisabledGroups.Contains(categoryName);
                        categories.Add(new SelectListGroup { Name = categoryName, Disabled = disabled });
                    }
                }
            }

            return categories;
        }
    }
    //TODO:Needs Refactor
    public class SelectListOptions
    {
        public SelectListOptions()
        {
            SelectedValues = new object[] { };
            DisabledValues = new object[] { };
            DisabledGroups = new string[] { };
        }

        public object[] SelectedValues { get; set; }
        public object[] DisabledValues { get; set; }
        public string[] DisabledGroups { get; set; }
        public string Placeholder { get; set; }
        public bool IsStringValue { get; set; }

    }
}
