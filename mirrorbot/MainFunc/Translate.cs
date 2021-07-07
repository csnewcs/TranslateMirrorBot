using System;
using System.IO;
using System.Text;
using System.Web;
using System.Net;
using Newtonsoft.Json.Linq;

namespace mirrorbot
{
    public class Papago
    {
        readonly string NaverId;
        readonly string NaverSecret;
        public Papago(string id, string secret)
        {
            NaverId = id;
            NaverSecret = secret;
        }
        public string translate(string source, string target, string text)
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

            return translated;
        }
    }
}