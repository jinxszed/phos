using DSharpPlus;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace phos.Handlers.Dialogue.Steps
{
    public abstract class DialogueStepBase : IDialogueStep
    {
        protected readonly string _content;

        public DialogueStepBase(string content)
        {
            _content = content;
        }

        public Action<DiscordMessage> OnMessageAdded { get; set; } = delegate { };

        public abstract IDialogueStep NextStep { get; }

        public abstract Task<bool> ProcessStep(DiscordClient client, DiscordChannel channel, DiscordUser user);

        protected async Task TryAgain(DiscordChannel channel, string problem)
        {
            var embed_builder = new DiscordEmbedBuilder
            {
                Title = "Please try again",
                Color = DiscordColor.Red,
            };

            embed_builder.AddField("There was a problem with your previous input", problem);

            var embed = await channel.SendMessageAsync(embed: embed_builder).ConfigureAwait(false);

            OnMessageAdded(embed);
        }

    }
}
