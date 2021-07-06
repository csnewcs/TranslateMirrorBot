using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using SqlHelper;

namespace mirrorbot
{
    public class Ping : ModuleBase<SocketCommandContext>
    {

        [Command("핑")]
        public async Task help()
        {
            TimeSpan message = DateTime.Now - Context.Message.Timestamp;
            await ReplyAsync($"🏓! (디스코드 API:{Context.Client.Latency}ms, 메세지: {message.Milliseconds}ms)");
        }
    }
}