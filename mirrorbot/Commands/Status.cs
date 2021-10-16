using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SqlHelper;

namespace mirrorbot
{
    public class Status : ModuleBase<SocketCommandContext>
    {
        private readonly Translator _translator;
        private readonly MariaDB _db;
        public Status(Translator translator, MariaDB db) {_translator = translator; _db = db;}

        [Command("현황")]
        public async Task getStatus()
        {
            int[] used = _db.getUsed();
            // var used = _translator.getTranslatedText(); //{Papago, Kakao i}
            Thread tr = new Thread(() => {
                var img = makeImage(used[0], used[1]);
                img.SaveAsPng("nowstatus.png", new PngEncoder());
                Context.Channel.SendFileAsync("nowstatus.png");
            });
            tr.Start();
        }
        private SixLabors.ImageSharp.Image makeImage(int papago, int kakao)
        {
            int papagoSize = (int)((double)papago / 10000 * 304);
            int kakaoSize = (int)((double)kakao / 50000 * 304);
            Console.WriteLine($"Papago: {papago} / 10000 ({papagoSize}pixel)");
            Console.WriteLine($"Kakao: {kakao} / 50000 ({kakaoSize}pixel)");

            SixLabors.ImageSharp.Image img = SixLabors.ImageSharp.Image.Load("resources/Status.png");
            var fontCollection = new FontCollection();
            fontCollection.Install("resources/NanumSquareRoundB.ttf");
            var fontFamily = fontCollection.Families.First();
            // Console.WriteLine(fontFamily.AvailableStyles);
            var font = fontFamily.CreateFont(20, FontStyle.Regular);
            img.Mutate(i => {
                i.DrawText($"{papago}/10000", font, new SixLabors.ImageSharp.Color(new Rgba32(3, 0xC7, 0x5A)), new Point(272,117));
                i.DrawText($"{kakao}/50000", font, new SixLabors.ImageSharp.Color(new Rgba32(0xFA, 0xE1, 0)), new Point(272,236));
                i.Fill(new Rgba32(0x03, 0xC7, 0x5A), new Rectangle(272, 88, papagoSize, 29));
                i.Fill(new Rgba32(0xFA, 0xE1, 0), new Rectangle(272, 207, kakaoSize, 29));
            });
            return img;
        }
    }
}