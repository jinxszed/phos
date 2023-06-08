using DSharpPlus;
using DSharpPlus.Entities;
using phos.Handlers.Dialogue.Steps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace phos.Handlers.Dialogue
{
    public class DialogueHandler
    {
        private readonly DiscordClient _client;
        private readonly DiscordChannel _channel;
        private readonly DiscordUser _user;
        private IDialogueStep _current_step;

        public DialogueHandler(
            DiscordClient client,
            DiscordChannel channel,
            DiscordUser user,
            IDialogueStep starting_step)
        {
            _client = client;
            _channel = channel;
            _user = user;
            _current_step = starting_step;
        }

        private readonly List<DiscordMessage> messages = new List<DiscordMessage>();

        public async Task<bool> ProcessDialogue()
        {
            while (_current_step != null)
            {
                _current_step.OnMessageAdded += (message) => messages.Add(message); // add messages to dialogue list

                bool cancelled = await _current_step.ProcessStep(_client, _channel, _user).ConfigureAwait(false);

                if (cancelled)
                {
                    await DeleteMessages().ConfigureAwait(false);

                    var cancel_embed = new DiscordEmbedBuilder
                    {
                        Title = "Dialogue has successfully been cancelled",
                        Description = _user.Mention,
                        Color = DiscordColor.Green
                    };

                    await _channel.SendMessageAsync(embed: cancel_embed).ConfigureAwait(false);

                    return false;
                }

                _current_step = _current_step.NextStep;
            }

            await DeleteMessages().ConfigureAwait(false);

            return true;
        }
        private async Task DeleteMessages()
        {
            if (_channel.IsPrivate) { return; }

            foreach (var message in messages)
            {
                await message.DeleteAsync().ConfigureAwait(false);
            }
        }
    }
}
