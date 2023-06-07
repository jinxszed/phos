using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
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
    }
}
