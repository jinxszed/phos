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
    public class IntStep : DialogueStepBase
    {
        private readonly int? _min_value;
        private readonly int? _max_value;

        private IDialogueStep _next_step;

        public IntStep(
            string content,
            IDialogueStep next_step,
            int? min_length = null,
            int? max_length = null) : base(content)
        {
            _next_step = next_step;
            _min_value = min_length;
            _max_value = max_length;
        }

        public Action<int> OnValidResult{get; set; } = delegate { };

        public override IDialogueStep NextStep => _next_step;

        public void SetNextStep(IDialogueStep next_step)
        {
            _next_step = next_step;
        }

        public override async Task<bool> ProcessStep(DiscordClient client, DiscordChannel channel, DiscordUser user)
        {
            var embed_builder = new DiscordEmbedBuilder
            {
                Title = $"Please respond below",
                Description = $"{user.Mention}, {_content}",
            };

            embed_builder.AddField("To stop the dialogue", "Use the !cancel command");

            if (_min_value.HasValue)
            {
                embed_builder.AddField("Min Value:", $"{_min_value.Value}");
            }

            if (_max_value.HasValue)
            {
                embed_builder.AddField("Max Value:", $"{_max_value.Value}");
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

                if(!int.TryParse(message_result.Result.Content, out int input_value))
                {
                    await TryAgain(channel, "Your input is not an integer").ConfigureAwait(false);
                    continue;
                }

                if (_min_value.HasValue)
                {
                     if(input_value < _min_value.Value)
                    {
                        await TryAgain(channel, $"Your input value {input_value} is smaller than the mininmum of {_min_value}").ConfigureAwait(false);
                        continue;
                    }
                }
                
                if (_max_value.HasValue)
                {
                    if (input_value > _max_value.Value)
                    {
                        await TryAgain(channel, $"Your input value {input_value} is larger than the maximum of {_max_value}").ConfigureAwait(false);
                        continue;
                    }
                }

                OnValidResult(input_value);

                return false; // not cancelled
            }
        }
    }
}
