// Copyright (c) 2020 Pwn (Jonathan) / All rights reserved.

using System;

namespace EventSub.Lib.Attributes
{
    public class EventSubStatusAttribute : Attribute
    {
        public EventSubStatusAttribute(string status)
        {
            Status = status;
        }

        public string Status { get; }
    }
}