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
            try
            {

                SocketGuildUser guildUser = Context.User as SocketGuildUser;
                if(guildUser.GuildPermissions.Administrator || guildUser.Id == guildUser.Guild.OwnerId)
                {
                    string reply = "";
                    if(_db.dataExist("NoticeGuilds", "GuildID", Context.Guild.Id.ToString()))
                    {
                        _db.removeData("NoticeGuilds", "GuildID", Context.Guild.Id.ToString());
                        reply += "원래 공지가 오던 곳이 아닌 ";
                    }
                    string[] columns = new string[] {"GuildID", "ChannelID"};
                    string[] datas = new string[] {guildUser.Guild.Id.ToString(), Context.Channel.Id.ToString()};
                    _db.addData("NoticeGuilds", columns, datas);
                    reply += $"이 채널({Context.Channel.Name})에 공지가 오도록 설정했어요.";
                    await ReplyAsync(reply);
                }
                else
                {
                    await ReplyAsync("이 명령어는 관리자만 사용 가능해요.");
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }
        [Command("공지삭제")]
        public async Task removeNoticeChannel()
        {
            SocketGuildUser guildUser = Context.User as SocketGuildUser;
            if(guildUser.GuildPermissions.Administrator || guildUser.Id == guildUser.Guild.OwnerId)
            {
                if(_db.getData("NoticeGuilds", "ChannelID", "GuildID", Context.Guild.Id.ToString()) == null)
                {
                    await ReplyAsync("이 서버엔 공지가 가는 채널이 없어요.");
                }
                else
                {
                    _db.removeData("NoticeGuilds", "GuildID", Context.Guild.Id.ToString());
                    await ReplyAsync("이제 이 서버에는 공지가 오지 않아요.");
                }
            }
            else
            {
                await ReplyAsync("이 명령어는 관리자만 사용 가능해요.");
            }
        }
    }
}