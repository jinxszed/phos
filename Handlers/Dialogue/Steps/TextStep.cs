using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace phos.Handlers.Dialogue.Steps
{
    public class TextStep : DialogueStepBase
    {
        private readonly int? _min_length;
        private readonly int? _max_length;
        private IDialogueStep _next_step;

        public TextStep(
            string content,
            IDialogueStep next_step,
            int? min_length = null,
            int? max_length = null) : base(content)
        {
            _next_step = next_step;
            _min_length = min_length;
            _max_length = max_length;
        }

        public Action<string> OnValidResult{get; set; } = delegate { };

        public override IDialogueStep NextStep => _next_step;

        public override async Task<bool> ProcessStep(DiscordClient client, DiscordChannel channel, DiscordUser user)
        {
            var embed_builder = new DiscordEmbedBuilder
            {
                Title = $"Please respond below",
                Description = $"{user.Mention}, {_content}",
            };

            embed_builder.AddField("To stop the dialogue", "Use the !cancel command");

            if (_min_length.HasValue)
            {
                embed_builder.AddField("Min length:", $"{_min_length.Value} characters");
            }

            if (_max_length.HasValue)
            {
                embed_builder.AddField("Max length:", $"{_max_length.Value} characters");
            }

            var interactivity = client.GetInteractivity();

            while (true)
            {
                var embed = await channel.SendMessageAsync(embed: embed_builder).ConfigureAwait(false);

                OnMessageAdded(embed);

                var message_result = await interactivity.WaitForMessageAsync(
                    x => x.ChannelId == channel.Id && x.Author.Id == user.Id).ConfigureAwait(false);

                OnMessageAdded(message_result.Result);

                if(message_result.Result.Content.Equals("!cancel", StringComparison.OrdinalIgnoreCase))
                {
                    return true; // cancelled
                }

                if (_min_length.HasValue)
                {
                     if(message_result.Result.Content.Length < _min_length.Value)
                    {
                        await TryAgain(channel, $"Your input is {_min_length.Value - message_result.Result.Content.Length} characters too short").ConfigureAwait(false);
                        continue;
                    }
                }
                
                if (_max_length.HasValue)
                {
                    if (message_result.Result.Content.Length > _max_length.Value)
                    {
                        await TryAgain(channel, $"Your input is {message_result.Result.Content.Length - _max_length.Value} characters too long").ConfigureAwait(false);
                        continue;
                    }
                }

                OnValidResult(message_result.Result.Content);

                return false; // not cancelled
            }
        }
    }
}
