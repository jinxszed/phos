using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Collections.ObjectModel;

namespace phos.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class RequireCategoriesAttributes : CheckBaseAttribute
    {
        public IReadOnlyList<string> CategoryNames { get; }

        // defines channel requirements; usage can be for limiting to staff channels, a specific channel or thread, etc.
        public ChannelCheckMode check_mode { get; }
        public RequireCategoriesAttributes(ChannelCheckMode chk_mode, params string[] channel_names)
        {
            check_mode = chk_mode;
            CategoryNames = new ReadOnlyCollection<string>(channel_names);
        }
        public override Task<bool> ExecuteCheckAsync(CommandContext context, bool help)
        {
            if (context.Guild == null || context.Member == null)
            {
                return Task.FromResult(false);
            }

            bool contains = CategoryNames.Contains(context.Channel.Parent.Name, StringComparer.OrdinalIgnoreCase);

            return check_mode switch
            {
                ChannelCheckMode.Any => Task.FromResult(contains),
                ChannelCheckMode.None => Task.FromResult(!contains),
                _ => Task.FromResult(false), // _ is the discards variable, i.e. a way to ignore the result completely
                                             // here it's a stand-in for MineOrParentAny, because we don't care about that value
            };
        }
    }
}
