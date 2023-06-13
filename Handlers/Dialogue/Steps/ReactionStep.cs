using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using phos.Handlers.Dialogue.Steps;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace phos.Bots.Handlers.Dialogue.Steps
{
    public class ReactionStep : DialogueStepBase
    {
        private readonly Dictionary<DiscordEmoji, ReactionStepData> _options;

        private DiscordEmoji _selected_emoji;

        public ReactionStep(string content, Dictionary<DiscordEmoji, ReactionStepData> options) : base(content)
        {
            _options = options;
        }

        public override IDialogueStep NextStep => _options[_selected_emoji].NextStep;

        public Action<DiscordEmoji> OnValidResult { get; set; } = delegate { };

        public override async Task<bool> ProcessStep(DiscordClient client, DiscordChannel channel, DiscordUser user)
        {
            var cancel_emoji = DiscordEmoji.FromName(client, ":x:");

            var embed_builder = new DiscordEmbedBuilder
            {
                Title = $"Please React To This Embed",
                Description = $"{user.Mention}, {_content}",
            };

            embed_builder.AddField("To Stop The Dialogue", "React with the :x: emoji");

            var interactivity = client.GetInteractivity();

            while (true)
            {
                var embed = await channel.SendMessageAsync(embed: embed_builder).ConfigureAwait(false);

                OnMessageAdded(embed);

                foreach (var emoji in _options.Keys)
                {
                    await embed.CreateReactionAsync(emoji).ConfigureAwait(false);
                }

                await embed.CreateReactionAsync(cancel_emoji).ConfigureAwait(false);

                var reaction_result = await interactivity.WaitForReactionAsync(
                    x => _options.ContainsKey(x.Emoji) || x.Emoji == cancel_emoji,
                    embed,
                    user
                    ).ConfigureAwait(false);

                if (reaction_result.Result.Emoji == cancel_emoji)
                {
                    return true;
                }

                _selected_emoji = reaction_result.Result.Emoji;

                OnValidResult(_selected_emoji);

                return false;
            }
        }

        public class ReactionStepData
        {
            public string Content { get; set; }
            public IDialogueStep NextStep { get; set; }


        }

    }
}