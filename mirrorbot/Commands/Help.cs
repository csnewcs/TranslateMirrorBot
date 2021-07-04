using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using SqlHelper;

namespace mirrorbot
{
    public class Help : ModuleBase<SocketCommandContext>
    {
        MariaDB _mariadb;
        public Help(MariaDB mariaDB) {_mariadb = mariaDB;}

        [Command("도움")]
        public async Task help()
        {
            await ReplyAsync("응답!");
        }
    }
}