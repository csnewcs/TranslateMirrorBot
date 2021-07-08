using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using SqlHelper;

namespace mirrorbot
{
    class SendNotice
    {
        public async Task sendNotice(MariaDB db, DiscordSocketClient client,string message)
        {
            var reader = db.getAllTableData("NoticeGuilds");
            Dictionary<ulong, ulong> dict = new Dictionary<ulong, ulong>(); //GuildID, ChannelID
            while(reader.Read())
            {
                dict.Add(Convert.ToUInt64(reader["GuildID"]), Convert.ToUInt64(reader["ChannelID"]));
            }
            reader.Close();
            int succeed = 0;
            int failed = 0;
            foreach(var keyvalue in dict)
            {
                try
                {
                    SocketTextChannel channel = client.GetGuild(keyvalue.Key).GetTextChannel(keyvalue.Value);
                    await channel.SendMessageAsync(message);
                    succeed++;
                }
                catch
                {
                    failed++;
                }
            }
            Console.WriteLine($"{dict.Count}개의 서버 중 {succeed}개의 서버에 공지 전송 완료({failed}개의 서버 전송 실패)");
        }
    }
}