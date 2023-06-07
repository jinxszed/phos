using System;
using System.Threading.Tasks;
using DSharpPlus;

namespace Phos
{
    class Program
    {
        static async Task Main(string[] args)
        {
            

            var discord = new DiscordClient(new DiscordConfiguration()
            {
                Token = "", //DELETE THIS ASAP
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents
            });

            discord.MessageCreated += async (s, e) =>
            {
                if (e.Message.Content.ToLower().StartsWith("ping"))
                    await e.Message.RespondAsync("test test!");
            };

            await discord.ConnectAsync();
            await Task.Delay(-1);
        }
    }
}
