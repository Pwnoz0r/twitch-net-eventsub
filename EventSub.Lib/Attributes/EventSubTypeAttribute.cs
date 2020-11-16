// Copyright (c) 2020 Pwn (Jonathan) / All rights reserved.

using System;

namespace EventSub.Lib.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public class EventSubTypeAttribute : Attribute
    {
        public EventSubTypeAttribute(string type, string scope = default)
        {
            Type = type;
            Scope = scope;
        }

        public string Type { get; }

        public string Scope { get; }
    }
}