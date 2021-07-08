using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using SqlHelper;

namespace mirrorbot
{
    public class RemoveChannel : ModuleBase<SocketCommandContext>
    {
        MariaDB _mariadb;
        public RemoveChannel(MariaDB mariaDB) {_mariadb = mariaDB;}

        [Command("번역삭제")]
        public async Task help()
        {
            if(_mariadb.getData("guild_" + Context.Guild.Id, "StartChannel", Context.Channel.Id, "StartLang") == null)
            {
                await ReplyAsync("이 채널은 번역 시작 설정이 되어있지 않아요.");
                return;
            }
            SocketGuildUser guildUser = Context.User as SocketGuildUser;
            if(guildUser.GuildPermissions.Administrator || guildUser.Guild.Owner == guildUser)
            {
                _mariadb.removeData("guild_" + Context.Guild.Id, "StartChannel", Context.Channel.Id);
                await ReplyAsync("번역 채널 삭제가 완료되었어요. 이제 자동으로 번역되지 않아요.");
            }
            else
            {
                await ReplyAsync("이 명령어는 관리자 권한 이상이어야 사용 가능해요.");
            }
        }
    }
}