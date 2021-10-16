using System;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using Discord;
using Discord.WebSocket;
using SqlHelper;
using Newtonsoft.Json.Linq;

namespace mirrorbot
{
    class SendTranslate
    {
        MariaDB db;
        Translator translator;
        public SendTranslate(Translator _translator, MariaDB _db) {translator = _translator; db = _db;}
        public SendTranslate() {}
        public void setRequires(Translator _translator, MariaDB _db) {translator = _translator; db = _db;}
        
        public async Task sendToAnotherChannel(SocketGuildChannel channel, SocketUserMessage message)
        {
            if(!isEnabled(channel) || message.Content == "") return;
            string startLang;
            string endLang;
            SocketTextChannel textChannel = getInfo(channel, out startLang, out endLang);
            JObject translated = translator.translate(startLang, endLang, message.Content);

            if(translated["translated"] == null) return;
            var user = message.Author as SocketGuildUser;
            EmbedAuthorBuilder authorBuilder = new EmbedAuthorBuilder()
                .WithName((user.Nickname == null ? user.Username : user.Nickname) + $", #{channel.Name}")
                .WithIconUrl(user.GetAvatarUrl());
            EmbedBuilder builder = new EmbedBuilder()
            .WithAuthor(authorBuilder)
            .AddField($"{startLang} -> {endLang}", translated["translated"])
            .AddField("Translated By",  $"{translated["translator"]}");
            await textChannel.SendMessageAsync("", false, builder.Build());
        }
        private bool isEnabled(SocketGuildChannel channel)
        {
            if(db.tableExits("guild_" + channel.Guild.Id))
            {
                return db.getData("guild_" + channel.Guild.Id, "StartChannel", channel.Id, "EndLang") != null;
            }
            return false;
        }
        private SocketTextChannel getInfo(SocketGuildChannel channel, out string startLang, out string endLang)
        {
            Dictionary<string, object> data = db.getChannelData(channel.Id, channel.Guild.Id);
            startLang = data["StartLang"].ToString();
            SocketTextChannel endChannel = channel.Guild.GetTextChannel(Convert.ToUInt64(data["EndChannel"].ToString()));
            endLang = data["EndLang"].ToString();
            return endChannel;
            // string translated = _papago.translate(startLang, endLang, msg.Content);
        }
    }
}