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

        [Command("도움")]
        public async Task help()
        {
            EmbedBuilder builder = new EmbedBuilder()
                .AddField("ㅂ!번역시작", "해당 채널에서 번역을 시작해요. 이후 안내를 따라 이모지를 넣어 주세요.(관리자만 사용 가능해요.)")
                .AddField("ㅂ!번역도착", "번역시작 채널에서 번역한 것을 봇이 올리는 채널을 설정해요. ㅂ!번역시작 을 완전히 끝낸 후 입력해 주세요.")
                .AddField("ㅂ!번역삭제", "해당 채널에서 출발하는 번역을 삭제해요.(관리자만 사용 가능해요.)")
                .WithFooter("csnewcs제작, Github: https://github.com/csnewcs/translatemirrorbot");
            await ReplyAsync("", false, builder.Build());
        }
    }
}