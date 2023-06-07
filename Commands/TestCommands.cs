using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace phos.Commands
{
    public class TestCommands : BaseCommandModule
    {
        [Command("ping")]
        [Description("Responds with pong or some other quirky message.")]
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
        public async Task ResponedReaction(CommandContext context)
        {
            var interactivity = context.Client.GetInteractivity();

            var message = await interactivity
                .WaitForReactionAsync(x => x.Channel == context.Channel)
                .ConfigureAwait(false);

            await context.Channel.SendMessageAsync(message.Result.Emoji);
        }

    }
}
