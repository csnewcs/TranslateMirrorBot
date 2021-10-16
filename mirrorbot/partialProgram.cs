using System;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Threading;
using SharpKoreanBots.Bot;

namespace mirrorbot
{
    public partial class Program
    {
        void init()
        {
            JObject json = new JObject();
            Console.WriteLine("초기 설정을 시작합니다.");
            Console.WriteLine("봇의 토큰을 입력해 주세요");
            json.Add("token", Console.ReadLine());
            Console.WriteLine("네이버 API의 ID를 입력해 주세요");
            json.Add("naverId", Console.ReadLine());
            Console.WriteLine("네이버 API의 secret을 입력해 주세요");
            json.Add("naverSecret", Console.ReadLine());
            Console.WriteLine("카카오 애플리케이션의 RestAPIKey를 입력해 주세요");
            json.Add("kakaoKey", Console.ReadLine());
            File.WriteAllText("config.json", json.ToString());
        }
        async void command(string command)
        {
            //콘솔에서의 커맨드
            //notice: notice.txt에 있는 내용을 여러 서버들의 공지 받는 곳으로 보냄
            //shutdown: 봇을 꺼버림
            if(command == "notice")
            {
                SendNotice notice = new SendNotice();
                await notice.sendNotice(_mariaDB, _client, File.ReadAllText("notice.txt"));
            }
            else if(command == "shutdown")
            {
                Console.WriteLine("bye");
                Environment.Exit(0);
            }
        }

        void update(string token, ulong id)
        {
            SharpKoreanBots.KoreanBotsClient client = new SharpKoreanBots.KoreanBotsClient(token);
            var botInfo = client.GetBot(id);
            botInfo.Token = token;
            // DateTime dt = DateTime.Now;
            while(true)
            {
                botInfo.ServerCount = _client.Guilds.Count;
                Logging.Log.log($"봇 정보 업데이트 / 서버 수: {_client.Guilds.Count}");
                botInfo.Update();
                Thread.Sleep(3600000); //1시간 새로고침
            }
        }
    }
}