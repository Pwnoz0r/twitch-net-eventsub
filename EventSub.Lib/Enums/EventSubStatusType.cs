// Copyright (c) 2020 Pwn (Jonathan) / All rights reserved.

using EventSub.Lib.Attributes;

namespace EventSub.Lib.Enums
{
    public enum EventSubStatusType
    {
        None,
        [EventSubStatus("enabled")] Enabled,

        [EventSubStatus("webhook_callback_verification_pending")]
        Pending,

        [EventSubStatus("webhook_callback_verification_failed")]
        Failed,

        [EventSubStatus("notification_failures_exceeded")]
        FailuresExceeded,

        [EventSubStatus("authorization_revoked")]
        Revoked,
        [EventSubStatus("user_removed")] Removed
    }
}