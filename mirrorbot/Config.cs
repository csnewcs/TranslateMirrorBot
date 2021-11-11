using System.IO;
using Newtonsoft.Json.Linq;

namespace mirrorbot
{
    struct Config
    {
        static string _discordToken;
        public static string DiscordToken
        {
            get { return _discordToken; }
        }
        static string _koreanBotListToken;
        public static string KoreanBotListToken
        {
            get { return _koreanBotListToken; }
        }
        static string _naverId;
        public static string NaverId
        {
            get { return _naverId; }
        }
        static string _naverSecret;
        public static string NaverSecret
        {
            get { return _naverSecret; }
        }
        static string _kakaoKey;
        public static string KakaoKey
        {
            get { return _kakaoKey; }
        }
        static int _channelLimit;
        public static int ChannelLimit
        {
            get { return _channelLimit; }
        }
        public static void Init()
        {
            string path = "./config.json";
            FileInfo info = new FileInfo(path);
            if(info.Exists)
            {
                JObject json = JObject.Parse(File.ReadAllText(path));
                _discordToken = json["token"]?.ToString();
                _koreanBotListToken = json["koreanBotListToken"]?.ToString();
                _naverId = json["naverId"]?.ToString();
                _naverSecret = json["naverSecret"]?.ToString();
                _kakaoKey = json["kakaoKey"]?.ToString();
                _channelLimit = int.Parse(json["channelLimit"]?.ToString());
            }
            else
            {
                Program.init();
                Init();
            }
        }
    }
}