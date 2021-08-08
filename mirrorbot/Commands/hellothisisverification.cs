using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace mirrorbot
{
    public class hello : ModuleBase<SocketCommandContext>
    {
        [Command("hellothisisverification")]
        public async Task hellothisisverification()
        {
            await ReplyAsync("csnewcs#0001");
        }
    }
}