# phos_csharp

Discord bot named phos in C#\
*CURRENTLY FOLLOWING A TUTORIAL, MOST CONTENTS ARE NOT ORIGINAL*\
Credit for tutorial: [DapperDino](https://github.com/dapperDino/)\
Tutorial repository [here](https://github.com/DapperDino/Discord-Bot-Tutorial/)

## Goals

- Deepen understanding of C#, network communications, windows development, and database interaction, and develop my first proper application
- Familiarize with Discord API through DSharpPlus, build working knowledge of it to be able to translate it to other languages and environments such as TypeScript with Deno

## Initial Functions

- Basic moderation commands, including but not limited to:
    - Add/remove roles
    - Issue/edit/delete warnings to users
    - Kick/Ban/Unban users
    - Record logs
    - Edit channel permissions (ex. lock down for non-moderators)
- Chat interaction
    - Respond to certain messages or patterns with a message (ex. user: "hello" bot: "what's up")
    - DM interaction support

## Bot Function Goal

**True goal is to make the bot specialized for chat replay.**\
The desired function can be illustrated as follows:

>Being given two parameters, an initial message link and an end message link, outputs the messages sent in that channel in order from oldest to newest, sent at the same intervals as they were sent originally. \
For example, User A sends the message "AHOY!" -> 3ms elapse -> User B sends "AHOY!!!" -> 2s elapse -> User C sends "lol." \
phos will replay those messages as they were originally sent. 

### Potential Issues

- Might be punished for too many API requests in short succession
- External emotes and animated emotes and stickers won't output unless the bot has Nitro  
- (add more predictions here)

## Tentative Outline and Example

Essential functions: setreplay/play/stop\
Convenience functions: pause/speed (change speed of messages up to 2x)\
Prefix: "p!" (currently "!" for quicker testing)

- only usable in `#dedicated_channel`
- only member who used command for that thread can use commands in that thread (prevent trolling)
- p!setreplay only accepts 2 input from #stream_chat (message links)
- upon `p!setreplay <start> <end>`, responds with `Created thread: #thread_name` and creates thread named `#chat_replay_[counter]`
- `[counter]` starts at 1 and increments depending on how many threads starting with `chat_replay_` threads there are (or could just keep incrementing to make it easier, but would look sloppier)
- creates thread in channel and immediately mentions the user who used `p!setreplay` in order to add them to the thread automatically
    - outputs a small embed with commands (play/pause/stop) and explanation
- thread set to expire after 1 hour (or 30 minutes)
    - phos deletes thread immediately after thread is closed
        - make sure phos doesn't delete the channel itself, only the thread 
- make sure phos never mentions users in chat replay (if someone sends `@jinxzed` in chat, phos will send `@/jinxszed` so as to not mention user jinxszed)
- handle external emotes and stickers (easy? have to investigate)
- (TENTATIVE IDEA) grabs message from start to end, inclusively, and stores message contents and timestamp in a table(? or hash map or dictionary idk), maybe could be done without a data structure. probably.

## References

- [D#+ Documentation](https://dsharpplus.github.io/DSharpPlus/)
- [Fundamental Data Structures and Algorithms in C#](https://dev.to/adavidoaiei/fundamental-data-structures-and-algorithms-in-c-4ocf)
- [Hashtables](https://dev.to/adavidoaiei/fundamental-data-structures-and-algorithms-in-c-4ocf#hashtable)
- [D#+ tutorial followed](https://www.youtube.com/playlist?list=PLS6sInD7ThM0Zb8F_KBl4T_jGF1e3apsc)