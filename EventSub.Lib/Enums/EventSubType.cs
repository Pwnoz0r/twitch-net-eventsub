// Copyright (c) 2020 Pwn (Jonathan) / All rights reserved.

using EventSub.Lib.Attributes;

namespace EventSub.Lib.Enums
{
    public enum EventSubType
    {
        [EventSubType("channel.update")] ChannelUpdate,
        [EventSubType("channel.follow")] ChannelFollow,
        [EventSubType("channel.subscribe")] ChannelSubscribe,

        [EventSubType("channel.cheer", "bits:read")]
        ChannelCheer,

        [EventSubType("channel.ban", "channel:moderate")]
        ChannelBan,

        [EventSubType("channel.unban", "channel:moderate")]
        ChannelUnban,

        [EventSubType("channel.channel_points_custom_reward.add", "channel:read:redemptions")]
        ChannelPointsRewardAdd,

        [EventSubType("channel.channel_points_custom_reward.update", "channel:read:redemptions")]
        ChannelPointsRewardUpdate,

        [EventSubType("channel.channel_points_custom_reward.remove", "channel:read:redemptions")]
        ChannelPointsRewardRemove,

        [EventSubType("channel.channel_points_custom_reward_redemption.add", "channel:read:redemptions")]
        ChannelPointsRewardRedemptionAdd,

        [EventSubType("channel.channel_points_custom_reward_redemption.update", "channel:read:redemptions")]
        ChannelPointsRewardRedemptionUpdate,

        [EventSubType("channel.hype_train.begin", "channel:read:hype_train")]
        ChannelHypeTrainBegin,

        [EventSubType("channel.hype_train.progress", "channel:read:hype_train")]
        ChannelHypeTrainProgress,

        [EventSubType("channel.hype_train.end", "channel:read:hype_train")]
        ChannelHypeTrainEnd,

        [EventSubType("stream.online")] StreamOnline,

        [EventSubType("stream.offline")] StreamOffline,

        [EventSubType("user.authorization.revoke")]
        UserAuthorizationRevoke,

        [EventSubType("user.update", "user:read:email")]
        UserUpdate
    }
}