// Copyright (c) 2020 Pwn (Jonathan) / All rights reserved.

using System;
using System.Linq;

namespace EventSub.Lib.Extensions
{
    public static class EnumExtension
    {
        // https://stackoverflow.com/a/9276348
        public static T GetAttributeOfType<T>(this Enum enumVal) where T : Attribute
        {
            var type = enumVal.GetType();
            var memInfo = type.GetMember(enumVal.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
            return attributes.Length > 0 ? (T) attributes[0] : null;
        }

        public static TOne GetValueFromDescription<TOne, TTwo>(this string description, string propertyName)
            where TOne : Enum where TTwo : Attribute
        {
            var temp = typeof(TOne)
                .GetFields()
                .ToList();

            foreach (var fieldInfo in temp)
            {
                var enumType = typeof(TTwo);

                var attribute = Attribute.GetCustomAttribute(fieldInfo, enumType);
                if (attribute == null)
                    continue;

                var value = attribute.GetType().GetProperty(propertyName)?.GetValue(attribute, null)?.ToString();

                if (string.IsNullOrEmpty(value))
                    return default;

                if (value == description)
                    return (TOne) fieldInfo.GetValue(null);
            }

            return default;
        }
    }
}