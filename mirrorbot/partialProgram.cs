using System;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Threading;

using SharpKoreanBots.Bot;

namespace mirrorbot
{
    public partial class Program
    {
        public static void init()
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
            Console.WriteLine("서버당 번역 채널을 몇 개로 제한할까요? (기본: 1)");
            string limit = Console.ReadLine();
            json.Add("channelLimit", limit == "" ? 1 : int.Parse(limit));
            Console.WriteLine("한국 디스코드 리스트에 등록된 봇인가요? (있다면 토큰을 입력해 주세요. 없으면 Enter를 눌러 주세요.)");
            json.Add("koreanBotListToken", Console.ReadLine());
            Console.WriteLine($"초기 설정이 완료되었습니다.\n====================설정값====================\n디스코드\n\t토큰: {json["token"]}\n\t한국 디스코드 봇 리스트 등록 여부: {json["koreanBotListToken"]}\n\t서버당 채널 제한: {json["channelLimit"]}\n번역\n\t네이버 API ID: {json["naverId"]}\n\t네이버 API Secret: {json["naverSecret"]}\n\t카카오 RestAPI Key: {json["kakaoKey"]}\n===========================================");
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
            var botInfo = BotInfo.Get(id);
            botInfo.Token = token;
            // DateTime dt = DateTime.Now;
            while(true)
            {
                try
                {
                    botInfo.ServerCount = _client.Guilds.Count;
                    Logging.Log.log($"봇 정보 업데이트 / 서버 수: {_client.Guilds.Count}");
                    botInfo.Update();
                }
                catch{

                }
                Thread.Sleep(3600000); //1시간 새로고침
            }
        }
    }
}