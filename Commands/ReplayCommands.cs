using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace phos.commands
{
    public class ReplayCommands : BaseCommandModule
    {
        [Command("setreplay")]
        [Description("Sets start and end points of chat replay. Accepts 2 and only 2 arguments, both of them being message links.")]
        public async Task SetReplay(CommandContext ctx)
        {
            Console.WriteLine("\nCalled setreplay.\n");
            await ctx.Channel.SendMessageAsync("Placeholder text. Proper command will follow the format `p!setreplay <message link> <message link>`.").ConfigureAwait(false); // placeholder line
        }
    }
}
