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
            await ReplyAsync("번역을 시작할 채널로 가서 'ㅂ!번역시작' 을 입력해 보세요");
        }
    }
}