using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;

public class License : ModuleBase<SocketCommandContext>
{
    [Command("라이센스")]
    public async Task showLicense()
    {
        EmbedFooterBuilder footerBuilder = new EmbedFooterBuilder();
        footerBuilder.Text = "MIT라이센스 보기: ";
        EmbedBuilder builder = new EmbedBuilder()
            .AddField(".Net 5.0", "https://github.com/dotnet/core / MIT License")
            .AddField("Discord.Net", "https://github.com/discord-net/Discord.Net / MIT License")
            .AddField("SixLabors.ImageSharp 1.0.3", "https://github.com/SixLabors/ImageSharp / Apache License 2.0")
            .AddField("SixLabors.ImageSharp.Drawing 1.0.0-beta13", "https://github.com/SixLabors/ImageSharp.Drawing / Apache License 2.0")
            .AddField("SixLabors.Fonts 1.0.0-beta15", "https://github.com/SixLabors/Fonts / Apche License 2.0");
        await ReplyAsync("", false, builder.Build());
    }
}