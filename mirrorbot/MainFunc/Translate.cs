using System;
using System.IO;
using System.Text;
using System.Web;
using System.Net;
using Newtonsoft.Json.Linq;
using SqlHelper;

namespace mirrorbot
{
    public class Translator
    {
        bool papago = true;
        bool kakaoi = true;
        private readonly string NaverId;
        private readonly string NaverSecret;
        private readonly string KakaoKey;
        int papagoTranslatedText = 0; //하루 1만자
        int kakaoiTranslatedText = 0; //하루 5만자
        JObject lang;
        DateTime endTime;

        public Translator(string naverId, string naverSecret, string kakaoKey)
        {
            NaverId = naverId;
            NaverSecret = naverSecret;
            KakaoKey = kakaoKey;
            lang = JObject.Parse(File.ReadAllText("translatorcode.json"));
        }
        public JObject translate(string source, string target, string text)
        {
            DateTime dt = DateTime.Now;
            if(dt.Date != endTime.Date)
            {
                (papago, kakaoi) = (true, true);
                (papagoTranslatedText, kakaoiTranslatedText) = (0, 0);
                endTime = dt;
            }
            JObject result = new JObject();
            
            if(papago) 
            {
                try
                {
                    result.Add("translated", naverPapago(lang[source]["papago"].ToString(), lang[target]["papago"].ToString(), text));
                    result.Add("translator", "Naver Papago");
                }
                catch
                {
                    papago = false;
                    result.Add("translated", kakaoITranslater(lang[source]["kakaoi"].ToString(), lang[target]["kakaoi"].ToString(), text));
                    result.Add("translator", "Kakao i Translator");
                    endTime = DateTime.Now;
                }
            }
            else if(kakaoi)
            {
                try
                {
                    result.Add("translated", kakaoITranslater(lang[source]["kakaoi"].ToString(), lang[target]["kakaoi"].ToString(), text));
                    result.Add("translator", "Kakao i Translator");
                }
                catch
                {
                    result.Add("Error", "Today's end");
                    endTime = DateTime.Now;
                }
            }
            else
            {
                result.Add("Error", "Today's end");
            }
            return result;
        }
        private string naverPapago(string source, string target, string text)
        {
            string[] splitByLine = text.Split('\n');
            string url = "https://openapi.naver.com/v1/papago/n2mt";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("X-Naver-Client-Id", NaverId);
            request.Headers.Add("X-Naver-Client-Secret", NaverSecret);
            request.Method = "POST";

            string[] endTranslate = new string[splitByLine.Length];
            for(int i = 0; i < splitByLine.Length; i++)
            {
                if(string.IsNullOrEmpty(splitByLine[i])) continue;
                request = (HttpWebRequest)WebRequest.Create(url);
                request.Headers.Add("X-Naver-Client-Id", NaverId);
                request.Headers.Add("X-Naver-Client-Secret", NaverSecret);
                request.Method = "POST";
                byte[] byteDataParams = Encoding.UTF8.GetBytes($"source={source}&target={target}&text=" + splitByLine[i]);
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = byteDataParams.Length;
                Stream st = request.GetRequestStream();
                st.Write(byteDataParams, 0, byteDataParams.Length);
                st.Close();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                string result = reader.ReadToEnd();
                stream.Close();
                response.Close();
                reader.Close();
                endTranslate[i] = JObject.Parse(result)["message"]["result"]["translatedText"].ToString();
            }
            string translated = string.Join('\n', endTranslate);
            papagoTranslatedText += text.Length;
            return translated;
        }
        private string kakaoITranslater(string source, string target, string text)
        {
            string url = $"https://dapi.kakao.com/v2/translation/translate?query={text}&src_lang={source}&target_lang={target}";
            WebClient client = new WebClient();
            client.Headers.Add("Authorization", "KakaoAK " + KakaoKey);
            string result = client.DownloadString(url);
            JArray resultJson = JObject.Parse(result)["translated_text"] as JArray;
            string turn = "";
            foreach(JArray line in resultJson)
            {
                foreach(JToken token in line)
                {
                    turn += token.ToString() + " "; //왜 배열 안에 배열이 있는진 모르겠지만 아무튼
                }
                turn += "\n";
            }
            kakaoiTranslatedText += text.Length;
            return turn;
        }
        public int[] getTranslatedText()
        {
            return new int[] {papagoTranslatedText, kakaoiTranslatedText};
        }
    }
}