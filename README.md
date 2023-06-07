# phos_csharp
Discord bot named phos in C#

## Goals
- Deepen understanding of C#, network communications, windows development, and database interaction, and develop my first proper application
- Familiarize with Discord API, build working knowledge of it to be able to translate it to other languages and environments such as TypeScript with Deno

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
True goal is to make the bot specialized for chat replay.\
The desired function is:\
    Being given two parameters, an initial message link and an end message link, outputs the messages sent in that channel in order from oldest to newest, sent at the same intervals as they were sent originally. \
    For example, User A sends the message "AHOY!" -> 3ms elapse -> User B sends "AHOY!!!" -> 2s elapse -> lol. \
    Phos Bot will replay those messages as they were originally sent. 

### Potential Issues
- Might be punished for too many API requests in short succession
- External emotes and animated emotes and stickers won't output unless the bot has Nitro and even normally they could be difficult to implement
- (add more predictions here)

