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

        [Command("도움")]
        public async Task help()
        {
            EmbedBuilder builder = new EmbedBuilder()
                .AddField("ㅂ!번역시작", "해당 채널에서 번역을 시작해요. 이후 안내를 따라 이모지를 넣어 주세요.(관리자만 사용 가능해요.)")
                .AddField("ㅂ!번역도착", "번역시작 채널에서 번역한 것을 봇이 올리는 채널을 설정해요. ㅂ!번역시작 을 완전히 끝낸 후 입력해 주세요.")
                .AddField("ㅂ!번역삭제", "해당 채널에서 출발하는 번역을 삭제해요.(관리자만 사용 가능해요.)")
                .AddField("ㅂ!공지설정", "해당하는 채널을 봇의 공지가 보내질 곳으로 설정해요.(관리자만 사용 가능해요.)")
                .AddField("ㅂ!공지삭제", "이 서버에 더이상 봇의 공지가 올라오지 않게 해요.(관리자만 이용 가능해요)")
                .AddField("ㅂ!핑", "현재 봇과 Discord API서버와의 핑, 메세지가 보내지고 봇이 인식하는 데 얼마나 걸리는 지 확인하는 명령어에요.")
                .AddField("ㅂ!현황", "현재 이 봇이 번역한 API별 글자 수를 알려줘요. 제한이 걸리는 것과 다소 차이가 있을 수 있아요.")
                .AddField("ㅂ!라이센스", "이 봇에 사용된 것들이 무엇인지, 또 그것들의 라이센스는 무엇인지 알려줘요.")
                .WithFooter("csnewcs제작, Github: https://github.com/csnewcs/translatemirrorbot");
            await ReplyAsync("", false, builder.Build());
        }
    }
}