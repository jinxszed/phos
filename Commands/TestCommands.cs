using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using phos.Attributes;
using phos.Handlers.Dialogue;
using phos.Handlers.Dialogue.Steps;

namespace phos.Commands
{
    public class TestCommands : BaseCommandModule
    {
        [Command("ping")]
        [Description("Responds with pong or some other quirky message.")]
        [RequireCategoriesAttributes(ChannelCheckMode.Any, "Text Channels")]
        public async Task Ping(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync("pong!")
                .ConfigureAwait(false);
        }

        [Command("add")]
        [Description("Adds two numbers.")]
        public async Task Add(CommandContext ctx,
            [Description("first number")] double num,
            [Description("second number")] double addend)
        {
            await ctx.Channel
                .SendMessageAsync((num + addend).ToString())
                .ConfigureAwait(false);
        }

        [Command("name")]
        [Description("Returns username and display name of member who uses this command. " +
            "Display name returns nothing if display name isn't explicitly set.")]
        public async Task UserName(CommandContext ctx)
        {
            await ctx.Channel
                .SendMessageAsync("username: " + ctx.Member.Username
                + "\ndisplay name: " + ctx.Member.Nickname)
                .ConfigureAwait(false);
        }

        // TODO
        [Command("messageid")]
        [Description("returns message id (WIP)")]
        [RequireRoles(RoleCheckMode.Any, "tweet")]
        public async Task MessageID(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync("filler").ConfigureAwait(false);
        }

        // this might be similiar to what i need to do the chat replay
        [Command("respondmessage")]
        [Description("Responds to user's input with the same input sent within 3 minutes.")]
        public async Task RespondMessage(CommandContext ctx)
        {
            var interactivity = ctx.Client.GetInteractivity();

            // x=>true, "x where true"; no matter what, it will always work (no prerequisite message)
            // this is changed to have a condition, that it must have been sent in the same channel as the ctx
            var message = await interactivity
                .WaitForMessageAsync(x => x.Channel == ctx.Channel)
                .ConfigureAwait(false);

            await ctx.Channel.SendMessageAsync(message.Result.Content);
        }

        [Command("respondreaction")]
        [Description("Responds to user's reaction with the same reaction as text sent within 3 minutes.")]
        public async Task RespondReaction(CommandContext ctx)
        {
            var interactivity = ctx.Client.GetInteractivity();

            var message = await interactivity
                .WaitForReactionAsync(x => x.Channel == ctx.Channel)
                .ConfigureAwait(false);

            await ctx.Channel.SendMessageAsync(message.Result.Emoji);
        }

        // TimeSpan class will definitely be needed for chat replay
        // poll does not exclude extraneous emojis

        [Command("poll")]
        [Description("Creates a poll")]
        public async Task Poll(CommandContext ctx, TimeSpan duration, params DiscordEmoji[] emoji_options)
        {
            var interactivity = ctx.Client.GetInteractivity();

            var options = emoji_options.Select(x => x.ToString());

            var poll_embed = new DiscordEmbedBuilder
            {
                Title = "Poll",
                Description = string.Join(" ", options)
            };

            var poll_message = await ctx.Channel.SendMessageAsync(embed: poll_embed).ConfigureAwait(false);
            foreach (var option in emoji_options)
            {
                await poll_message.CreateReactionAsync(option).ConfigureAwait(false);
            }

            var result = await interactivity.CollectReactionsAsync(poll_message, duration).ConfigureAwait(false);

            var poll_results = result.Select(x => $"{x.Emoji} : {x.Total}");

            await ctx.Channel.SendMessageAsync(string.Join("\n", poll_results)).ConfigureAwait(false);
        }

        [Command("dialogue")]
        [Description("starts a dialogue with the bot")]
        public async Task Dialogue(CommandContext ctx)
        {
            var input_step = new TextStep("Enter something interesting", null/*temp null*/);
            // var witty_step = new TextStep("Wow you're so funny", null);
            var int_step = new IntStep("haha funny", null, max_value: 100);

            string input = string.Empty;

            input_step.OnValidResult += (result) =>
            {
                input = result;

                if (result.ToLower() == "something interesting")
                {
                    // input_step.SetNextStep(witty_step);
                    input_step.SetNextStep(max_value); //************ HERE AT 16:31

                }
            }; // subscribe to input, add input to result 



            var user_channel = await ctx.Member.CreateDmChannelAsync().ConfigureAwait(false); // how to allow DMs with bot

            var input_dialogue_handler = new DialogueHandler(
                ctx.Client,
                user_channel,
                ctx.User,
                input_step
                );

            bool succeeded = await input_dialogue_handler.ProcessDialogue().ConfigureAwait(false);

            if(!succeeded) { return; }

            await ctx.Channel.SendMessageAsync(input).ConfigureAwait(false);

        }

    }
}
