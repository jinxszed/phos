using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace phos.Commands
{
    public class TeamCommands : BaseCommandModule
    {
        [Command("join")]
        // [Description("Responds with pong or some other quirky message.")]
        public async Task Join(CommandContext context)
        {
            var join_embed = new DiscordEmbedBuilder
            {
                Title = "Would you like to join?",
                Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail { Url = context.Client.CurrentUser.AvatarUrl }, // workaround to ThumbnailUrl not existing in current version of D#+
                Color = DiscordColor.Green
            };

            var join_message = await context.Channel.SendMessageAsync(embed: join_embed).ConfigureAwait(false);

            var thumbs_up_emoji = DiscordEmoji.FromName(context.Client, ":+1:");
            var thumbs_down_emoji = DiscordEmoji.FromName(context.Client, ":-1:");

            await join_message.CreateReactionAsync(thumbs_up_emoji).ConfigureAwait(false);
            await join_message.CreateReactionAsync(thumbs_down_emoji).ConfigureAwait(false);

            var interactivity = context.Client.GetInteractivity();

            var reaction_result = await interactivity.WaitForReactionAsync(
                x => x.Message == join_message && 
                x.User == context.User &&
                (x.Emoji == thumbs_up_emoji || x.Emoji == thumbs_down_emoji)).ConfigureAwait(false);

            if (reaction_result.Result.Emoji == thumbs_up_emoji)
            {
                var role = context.Guild.GetRole(990522104401264671);
                await context.Member.GrantRoleAsync(role).ConfigureAwait(false);
            }
            else
            {
                // something went wrong
            }
             await join_message.DeleteAsync().ConfigureAwait(false);
        }


    }
}
