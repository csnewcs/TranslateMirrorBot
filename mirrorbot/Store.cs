using System;
using System.Collections.Generic;
using Discord.WebSocket;
using Discord;
using Newtonsoft.Json.Linq;

public class Store
{
    JObject settingServers = new JObject();
    public bool isDoingServer(ulong serverId, ulong messageId = 0, ulong userId = 0)
    {
        if(settingServers.ContainsKey(serverId.ToString()))
        {
            JObject now = settingServers[serverId.ToString()] as JObject;
            if((ulong)now["message"] == messageId && (ulong)now["user"] == userId)
            {
                return true;
            }
            else if(messageId == 0 && userId == 0)
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
        JObject now = settingServers[serverId.ToString()] as JObject; //step = 0 or step = 1
        if((byte)now["step"] == 0)
        {
            returnString += "시작 언어를 ";
            string[] emojis = new string[12] {
                "🇰🇷", "🇺🇸", "🇯🇵", "🇨🇳", "🇻🇳", "🇮🇩", "🇹🇭", "🇩🇪", "🇷🇺", "🇪🇸", "🇮🇹", "🇫🇷"
            };
            if(emoji.Name == emojis[0]) //스위치 케이스가 왜 배열이 안되냐
            {
                returnString += "한국어로 설정했어요.";
                now.Add("startLang", "ko");
            }
            else if(emoji.Name == emojis[1])
            {
                returnString += "영어로 설정했어요.";
                now.Add("startLang", "en");
            }
            else if(emoji.Name == emojis[2])
            {
                returnString += "일본어로 설정했어요.";
                now.Add("startLang", "ja");
            }
            else if(emoji.Name == emojis[3])
            {
                returnString += "중국어(간체)로 설정했어요.";
                now.Add("startLang", "zh-CN");
            }
            else if(emoji.Name == emojis[4])
            {
                returnString += "베트남어로 설정했어요.";
                now.Add("startLang", "vi");
            }
            else if(emoji.Name == emojis[5])
            {
                returnString += "인도네시아어로 설정했어요.";
                now.Add("startLang", "id");
            }
            else if(emoji.Name == emojis[6])
            {
                returnString += "태국어로 설정했어요.";
                now.Add("startLang", "th");
            }
            else if(emoji.Name == emojis[7])
            {
                returnString += "독일로 설정했어요.";
                now.Add("startLang", "de");
            }
            else if(emoji.Name == emojis[8])
            {
                returnString += "러시아어로 설정했어요.";
                now.Add("startLang", "ru");
            }
            else if(emoji.Name == emojis[9])
            {
                returnString += "스페인어로 설정했어요.";
                now.Add("startLang", "es");
            }
            else if(emoji.Name == emojis[10])
            {
                returnString += "이탈리아어로 설정했어요.";
                now.Add("startLang", "it");
            }
            else if(emoji.Name == emojis[11])
            {
                returnString += "프랑스어로 설정했어요.";
                now.Add("startLang", "fr");
            }
            else //잘못된 이모지
            {
                returnString = "잘못된 언어를 선택하셧서요. 다시 선택해 주세요.";
                return returnString;
            }
            now["step"] = (byte)now["step"] + 1;
            returnString += "\n이제 번역된 문장이 쓰일 곳에 가서 'ㅂ!번역도착'을 입력해 주세요.";
        }
        else
        {
            returnString += "도착 언어를 ";
            string[] emojis = new string[12] {
                "🇰🇷", "🇺🇸", "🇯🇵", "🇨🇳", "🇻🇳", "🇮🇩", "🇹🇭", "🇩🇪", "🇷🇺", "🇪🇸", "🇮🇹", "🇫🇷"
            };
            if(emoji.Name == emojis[0]) //스위치 케이스가 왜 배열이 안되냐
            {
                returnString += "한국어로 설정했어요.";
                now.Add("endLang", "ko");
            }
            else if(emoji.Name == emojis[1])
            {
                returnString += "영어로 설정했어요.";
                now.Add("endLang", "en");
            }
            else if(emoji.Name == emojis[2])
            {
                returnString += "일본어로 설정했어요.";
                now.Add("endLang", "ja");
            }
            else if(emoji.Name == emojis[3])
            {
                returnString += "중국어(간체)로 설정했어요.";
                now.Add("endLang", "zh-CN");
            }
            else if(emoji.Name == emojis[4])
            {
                returnString += "베트남어로 설정했어요.";
                now.Add("endLang", "vi");
            }
            else if(emoji.Name == emojis[5])
            {
                returnString += "인도네시아어로 설정했어요.";
                now.Add("endLang", "id");
            }
            else if(emoji.Name == emojis[6])
            {
                returnString += "태국어로 설정했어요.";
                now.Add("endLang", "th");
            }
            else if(emoji.Name == emojis[7])
            {
                returnString += "독일로 설정했어요.";
                now.Add("endLang", "de");
            }
            else if(emoji.Name == emojis[8])
            {
                returnString += "러시아어로 설정했어요.";
                now.Add("endLang", "ru");
            }
            else if(emoji.Name == emojis[9])
            {
                returnString += "스페인어로 설정했어요.";
                now.Add("endLang", "es");
            }
            else if(emoji.Name == emojis[10])
            {
                returnString += "이탈리아어로 설정했어요.";
                now.Add("endLang", "it");
            }
            else if(emoji.Name == emojis[11])
            {
                returnString += "프랑스어로 설정했어요.";
                now.Add("endLang", "fr");
            }
            else //잘못된 이모지
            {
                returnString = "잘못된 언어를 선택하셧서요. 다시 선택해 주세요.";
                return returnString;
            }
            if(now["startLang"] == now["endLang"])
            {
                return "시작 언어와 도착 언어가 같아요. 도착 언어를 다시 설정해 주세요.";
            }
            SqlHelper.MariaDB db = new SqlHelper.MariaDB(); /*
            ServerID
            StartChannel StartLang  EndChannel  EndLang
            varchar(20)  varchar(2) varchar(20) varchar(2)
            */
            if(!db.tableExits(serverId.ToString()))
            {
                string[] columns = new string[] {
                    "StartChannel varchar(20)", "StartLang varchar(2)", "EndChannel varchar(20)", "EndLang varchar(2)"
                };
                db.createTable("guild_" + serverId.ToString(), columns);
            }
            string[] dataColumns = new string[] {
                "StartChannel", "StartLang", "EndChannel", "EndLang"
            };
            object[] datas = new object[] {
                now["startChannel"], now["startLang"], now["endChannel"], now["endLang"]
            };
            db.addData("guild_" + serverId.ToString(), dataColumns, datas);
            settingServers.Remove(serverId.ToString());
        }        
        return returnString;
    }
    public void endSetting(ulong serverId, ulong channelId, ulong messageId)
    {
        try
        {
            JObject now = settingServers[serverId.ToString()] as JObject;
            now.Add("endChannel", channelId);
            now["message"] = messageId;
            settingServers[serverId.ToString()] = now;
        }
        catch(Exception e)
        {
            Console.WriteLine(e);
        }
    }
}