using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using phos.Attributes;

namespace phos.Commands
{
    public class TestCommands : BaseCommandModule
    {
        [Command("ping")]
        [Description("Responds with pong or some other quirky message.")]
        [RequireCategoriesAttributes(ChannelCheckMode.Any, "Text Channels")]
        public async Task Ping(CommandContext context)
        {
            await context.Channel.SendMessageAsync("pong!")
                .ConfigureAwait(false);
        }

        [Command("add")]
        [Description("Adds two numbers.")]
        public async Task Add(CommandContext context,
            [Description("first number")] double num,
            [Description("second number")] double addend)
        {
            await context.Channel
                .SendMessageAsync((num + addend).ToString())
                .ConfigureAwait(false);
        }

        [Command("name")]
        [Description("Returns username and display name of member who uses this command. " +
            "Display name returns nothing if display name isn't explicitly set.")]
        public async Task UserName(CommandContext context)
        {
            await context.Channel
                .SendMessageAsync("username: " + context.Member.Username
                + "\ndisplay name: " + context.Member.Nickname)
                .ConfigureAwait(false);
        }

        // TODO
        [Command("messageid")]
        [Description("returns message id (WIP)")]
        [RequireRoles(RoleCheckMode.Any, "tweet")]
        public async Task MessageID(CommandContext context)
        {
            await context.Channel.SendMessageAsync("filler").ConfigureAwait(false);
        }

        // this might be similiar to what i need to do the chat replay
        [Command("respondmessage")]
        [Description("Responds to user's input with the same input sent within 3 minutes.")]
        public async Task RespondMessage(CommandContext context)
        {
            var interactivity = context.Client.GetInteractivity();

            // x=>true, "x where true"; no matter what, it will always work (no prerequisite message)
            // this is changed to have a condition, that it must have been sent in the same channel as the context
            var message = await interactivity
                .WaitForMessageAsync(x => x.Channel == context.Channel)
                .ConfigureAwait(false);

            await context.Channel.SendMessageAsync(message.Result.Content);
        }

        [Command("respondreaction")]
        [Description("Responds to user's reaction with the same reaction as text sent within 3 minutes.")]
        public async Task RespondReaction(CommandContext context)
        {
            var interactivity = context.Client.GetInteractivity();

            var message = await interactivity
                .WaitForReactionAsync(x => x.Channel == context.Channel)
                .ConfigureAwait(false);

            await context.Channel.SendMessageAsync(message.Result.Emoji);
        }

        // TimeSpan class will definitely be needed for chat replay
        // poll does not exclude extraneous emojis

        [Command("poll")]
        [Description("Creates a poll")]
        public async Task Poll(CommandContext context, TimeSpan duration, params DiscordEmoji[] emoji_options)
        {
            var interactivity = context.Client.GetInteractivity();

            var options = emoji_options.Select(x => x.ToString());

            var poll_embed = new DiscordEmbedBuilder
            {
                Title = "Poll",
                Description = string.Join(" ", options)
            };

            var poll_message = await context.Channel.SendMessageAsync(embed: poll_embed).ConfigureAwait(false);
            foreach (var option in emoji_options)
            {
                await poll_message.CreateReactionAsync(option).ConfigureAwait(false);
            };

            var result = await interactivity.CollectReactionsAsync(poll_message, duration).ConfigureAwait(false);

            var poll_results = result.Select(x => $"{x.Emoji} : {x.Total}");

            await context.Channel.SendMessageAsync(string.Join("\n", poll_results)).ConfigureAwait(false);
        }

    }
}
