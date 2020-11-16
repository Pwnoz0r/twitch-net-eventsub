// Copyright (c) 2020 Pwn (Jonathan) / All rights reserved.

using System;

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
    }
}