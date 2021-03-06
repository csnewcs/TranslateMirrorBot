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

        [Command("ν")]
        public async Task help()
        {
            TimeSpan message = DateTime.Now - Context.Message.Timestamp;
            await ReplyAsync($"π! (λμ€μ½λ API:{Context.Client.Latency}ms, λ©μΈμ§: {message.Milliseconds}ms)");
        }
    }
}