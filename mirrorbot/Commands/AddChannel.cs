using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace mirrorbot
{
    public class AddChannel : ModuleBase<SocketCommandContext>
    {
        Store _store;
        public AddChannel(Store store) => _store = store;

        [Command("번역시작")]
        public async Task StartTranslte()
        {
            if(_store.isDoingServer(Context.Guild.Id))
            {
                await ReplyAsync("이미 이 서버에서 설정 중인 것이 있어요. 그걸 끝내고 새로 설정해 주세요.");
            }
            var message = await ReplyAsync($"이 채널({Context.Message.Channel.Name})을 번역 출발 지점으로 설정했어요.\n반응을 추가하여 시작 언어를 선택해 주세요.");
            Emoji[] emojis = new Emoji[12] {
                new Emoji("🇰🇷"), new Emoji("🇺🇸"), new Emoji("🇯🇵"), new Emoji("🇨🇳"), new Emoji("🇻🇳"), new Emoji("🇮🇩"), new Emoji("🇹🇭"), new Emoji("🇩🇪"), new Emoji("🇷🇺"), new Emoji("🇪🇸"), new Emoji("🇮🇹"), new Emoji("🇫🇷")
            }; //한국어,             영어,           일본어,             중국어(간체),     베트남어,          인도네시아어,         태국어,          독일어,           러시아어,          스페인어,        이탈리아어,                 프랑스어
            _store.startSetting(Context.Guild.Id, Context.Channel.Id, Context.User.Id, message.Id);
            await message.AddReactionsAsync(emojis);
        }
        [Command("번역도착")]
        public async Task endTranslate()
        {
            var message = await ReplyAsync($"이 채널({Context.Channel.Name})을 번역 도착 지점으로 설정했어요.\n반응을 추가하여 도착 언어를 선택해 주세요.");
            Emoji[] emojis = new Emoji[12] {
                new Emoji("🇰🇷"), new Emoji("🇺🇸"), new Emoji("🇯🇵"), new Emoji("🇨🇳"), new Emoji("🇻🇳"), new Emoji("🇮🇩"), new Emoji("🇹🇭"), new Emoji("🇩🇪"), new Emoji("🇷🇺"), new Emoji("🇪🇸"), new Emoji("🇮🇹"), new Emoji("🇫🇷")
            }; //한국어,             영어,           일본어,             중국어(간체),     베트남어,          인도네시아어,         태국어,          독일어,           러시아어,          스페인어,        이탈리아어,                 프랑스어
            _store.endSetting(Context.Guild.Id, Context.Channel.Id, message.Id);
            await message.AddReactionsAsync(emojis);
        }
    }
}