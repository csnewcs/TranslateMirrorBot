using System;
using System.Threading.Tasks;
using System.IO;
using Discord;
using Discord.WebSocket;
using SqlHelper;

namespace mirrorbot
{
    class MainFunc
    {
        MariaDB db = new MariaDB();
        Papago papago;
        public MainFunc(Papago _papago) => papago = _papago;
        public async Task sendToAnotherChannel(SocketGuildChannel channel, SocketUserMessage message)
        {
            if(!isEnabled(channel)) return;
            string startLang;
            string endLang;
            SocketTextChannel textChannel = getInfo(channel, out startLang, out endLang);
            string translated = papago.translate(startLang, endLang, message.Content);
            var user = message.Author as SocketGuildUser;
            EmbedAuthorBuilder authorBuilder = new EmbedAuthorBuilder()
                .WithName((user.Nickname == null ? user.Username : user.Nickname) + $", #{channel.Name}")
                .WithIconUrl(user.GetAvatarUrl());
            EmbedBuilder builder = new EmbedBuilder()
            .WithAuthor(authorBuilder)
            .AddField($"{startLang} -> {endLang}", translated);
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
            startLang = db.getData("guild_" + channel.Guild.Id, "StartChannel", channel.Id, "StartLang").ToString();
            SocketTextChannel endChannel = channel.Guild.GetTextChannel(Convert.ToUInt64(db.getData("guild_" + channel.Guild.Id, "StartChannel", channel.Id, "EndChannel")));
            endLang = db.getData("guild_" + channel.Guild.Id, "StartChannel", channel.Id, "EndLang").ToString();
            return endChannel;
            // string translated = _papago.translate(startLang, endLang, msg.Content);
        }
    }
}