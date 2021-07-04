using System;
using System.Collections.Generic;
using Discord.WebSocket;
using Discord;
using Newtonsoft.Json.Linq;

public class Store
{
    JObject settingServers = new JObject();
    public bool isDoingServer(ulong serverId, ulong messageId, ulong userId)
    {
        if(settingServers.ContainsKey(serverId.ToString()))
        {
            JObject now = settingServers[serverId.ToString()] as JObject;
            if((ulong)now["message"] == messageId && (ulong)now["user"] == userId)
            {
                return true;
            }
        }
        return false;
    }
    public void startSetting(ulong serverId, ulong channelId, ulong userId, ulong messageId)
    {
        JObject now = new JObject();
        now.Add("user", userId);
        now.Add("startChannel", channelId);
        now.Add("message", messageId);
        now.Add("step", 0);
        settingServers.Add(serverId.ToString(), now);
    }
    public string emojiAdded(ulong serverId, ulong messageId, IEmote emoji, ulong userId)
    {
        string returnString = "";
        JObject now = settingServers[serverId.ToString()] as JObject; //step = 0 or step = 2
        if((byte)now["step"] == 0)
        {
            returnString += "시작 언어를 ";
            string[] emojis = new string[12] {
                "🇰🇷", "🇺🇸", "🇯🇵", "🇨🇳", "🇻🇳", "🇮🇩", "🇹🇭", "🇩🇪", "🇷🇺", "🇪🇸", "🇮🇹", "🇫🇷"
            };
            if(emoji.Name == emojis[0]) //스위치 케이스가 왜 배열이 안되냐
            {
                returnString += "한국어로 설정했습니다.";
                now.Add("startLang", "kr");
            }
            else if(emoji.Name == emojis[1])
            {
                returnString += "영어로 설정했습니다.";
                now.Add("startLang", "en");
            }
            else if(emoji.Name == emojis[2])
            {
                returnString += "일본어로 설정했습니다.";
                now.Add("startLang", "ja");
            }
            else if(emoji.Name == emojis[3])
            {
                returnString += "중국어(간체)로 설정했습니다.";
                now.Add("startLang", "zh-CN");
            }
            else if(emoji.Name == emojis[4])
            {
                returnString += "베트남어로 설정했습니다.";
                now.Add("startLang", "vi");
            }
            else if(emoji.Name == emojis[5])
            {
                returnString += "인도네시아어로 설정했습니다.";
                now.Add("startLang", "id");
            }
            else if(emoji.Name == emojis[6])
            {
                returnString += "태국어로 설정했습니다.";
                now.Add("startLang", "th");
            }
            else if(emoji.Name == emojis[7])
            {
                returnString += "독일로 설정했습니다.";
                now.Add("startLang", "de");
            }
            else if(emoji.Name == emojis[8])
            {
                returnString += "러시아어로 설정했습니다.";
                now.Add("startLang", "ru");
            }
            else if(emoji.Name == emojis[0])
            {
                returnString += "스페인어로 설정했습니다.";
                now.Add("startLang", "es");
            }
            else if(emoji.Name == emojis[0])
            {
                returnString += "이탈리아어로 설정했습니다.";
                now.Add("startLang", "it");
            }
            else if(emoji.Name == emojis[0])
            {
                returnString += "프랑스어로 설정했습니다.";
                now.Add("startLang", "fr");
            }
            else //잘못된 이모지
            {
                returnString = "잘못된 언어를 선택했습니다. 다시 선택해 주세요";
                return returnString;
            }
            now["step"] = (byte)now["step"] + 1;
            returnString += "\n이제 번역된 문장이 쓰일 곳에 가서 'ㅂ!번역도착'을 입력해 주세요";
        }
        return returnString;
    }
}