using DSharpPlus;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace phos.Handlers.Dialogue
{
    public interface IDialogueStep
    {
        Action<DiscordMessage> OnMessageAdded { get; set; } // can be used to clean up output by deleting unnecessary messages after the fact
        IDialogueStep NextStep { get; }
        Task<bool> ProcessStep(DiscordClient client, DiscordChannel channel, DiscordUser user);
    }
}
