using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using SqlHelper;

namespace mirrorbot
{
    public class NoticeChannel : ModuleBase<SocketCommandContext>
    {
        MariaDB _db;
        public NoticeChannel(MariaDB db) => _db = db;

        [Command("공지설정")]
        public async Task addNoticeChannel()
        {
            SocketGuildUser guildUser = Context.User as SocketGuildUser;
            if(guildUser.GuildPermissions.Administrator || guildUser.Id == guildUser.Guild.OwnerId)
            {
                string[] columns = new string[] {"GuildID", "ChannelID"};
                string[] datas = new string[] {guildUser.Guild.Id.ToString(), Context.Channel.Id.ToString()};
                _db.addData("NoticeGuilds", columns, datas);
                await ReplyAsync($"이 채널({Context.Channel.Name})을 공지가 오도록 설정했어요.");
            }
            else
            {
                await ReplyAsync("이 명령어는 관리자만 사용 가능해요.");
            }
        }
    }
}