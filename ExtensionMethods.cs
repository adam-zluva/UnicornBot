using Discord;

public static class ExtensionMethods
{
    public static MessageReference GetReference(this IMessage message)
    => message.Channel is IGuildChannel ?
        new(message.Id, message.Channel.Id, (message.Channel as IGuildChannel).GuildId) :
        new(message.Id, message.Channel.Id);
}