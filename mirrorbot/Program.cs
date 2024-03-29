﻿using System;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using System.Net;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Threading.Tasks;
using System.Text;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using Newtonsoft.Json.Linq;

using SqlHelper;

namespace mirrorbot
{
    public partial class Program
    {
        private static void Main(string[] args) => new Program().mainAsync().GetAwaiter().GetResult();
        
        DiscordSocketClient _client;
        CommandService _command;
        Microsoft.Extensions.DependencyInjection.ServiceProvider _services;
        MariaDB _mariaDB;
        Store _store;
        SendTranslate _SendTranslate;
        public async Task mainAsync()
        {
            Config.Init();
            // if(!File.Exists("log.txt")) File.WriteAllText("log.txt", "");
            DiscordSocketConfig clientConfig = new DiscordSocketConfig{MessageCacheSize = 100};

            _services = new ServiceCollection()
                .AddSingleton<DiscordSocketClient>(new DiscordSocketClient(clientConfig))
                .AddSingleton<CommandService>(new CommandService())
                .AddSingleton<SendTranslate>(new SendTranslate())
                .AddSingleton<Translator>(new Translator(Config.NaverId, Config.NaverSecret, Config.KakaoKey))
                .AddSingleton<Store>(new Store())
                .AddSingleton<MariaDB>(new MariaDB()).BuildServiceProvider();
            _client = _services.GetRequiredService<DiscordSocketClient>();
            _command = _services.GetRequiredService<CommandService>();
            _store = _services.GetRequiredService<Store>();
            _SendTranslate = _services.GetRequiredService<SendTranslate>();
            _mariaDB = _services.GetRequiredService<MariaDB>();
            _services.GetRequiredService<Translator>().SetDB(_mariaDB);
            _SendTranslate.setRequires(_services.GetRequiredService<Translator>(), _mariaDB);

            _client.MessageReceived += messageReceived;
            _client.ReactionAdded += reactionAdded;
            _client.Log += log;
            _client.Ready += ready;
            await _client.LoginAsync(TokenType.Bot, Config.DiscordToken);
            await _client.StartAsync();
            await _client.SetGameAsync("ㅂ!도움 으로 도움말 보기");
            await _command.AddModulesAsync(assembly: Assembly.GetEntryAssembly(), services: _services);
            while(true)
            {
                Console.WriteLine("명령어 사용하려면 입력");
                command(Console.ReadLine());
            }
        }
        private async Task messageReceived(SocketMessage msg)
        {
            if (msg.Author.IsBot || msg.Author.IsWebhook) return;
            if(msg.Channel is SocketGuildChannel)
            {
                SocketGuildChannel channel = msg.Channel as SocketGuildChannel;
                SocketGuild guild = channel.Guild;
                SocketUserMessage message = msg as SocketUserMessage;
                int argPos = 0;
                if(!message.HasStringPrefix("ㅂ!", ref argPos))
                {
                    await _SendTranslate.sendToAnotherChannel(channel, message);
                    return;
                }

                SocketCommandContext context = new SocketCommandContext(_client, message);
                var result = await _command.ExecuteAsync(context: context, argPos: argPos, services: _services);
                if(!result.IsSuccess) 
                {
                    Console.WriteLine(result.ErrorReason);
                    // await msg.Channel.SendMessageAsync(result.ErrorReason);
                }
            }
        }
        private async Task reactionAdded(Cacheable<IUserMessage, ulong> message, ISocketMessageChannel channel, SocketReaction reaction)
        {
            SocketGuildUser user = message.Value.Author as SocketGuildUser;
            if(user.Id != _client.CurrentUser.Id) return;
            if(_store.isDoingServer(user.Guild.Id, reaction.MessageId, reaction.UserId))
            {
                await message.Value.Channel.SendMessageAsync(_store.emojiAdded(user.Guild.Id, message.Id, reaction.Emote, user.Id));
            }
        }
        private Task ready()
        {
            DirectoryInfo info = new DirectoryInfo("./Log");
            if(!info.Exists)
            {
                info.Create();
            }
            if(Config.KoreanBotListToken != "")
            {
                Thread thread = new Thread(() => {
                    update(Config.KoreanBotListToken, _client.CurrentUser.Id);
                });
                thread.Start();
            }
            return Task.CompletedTask;
        }
        private Task log(LogMessage log)
        {
            Logging.Log.log(log.Message);
            return Task.CompletedTask;
            // return Task.CompletedTask;
        }

    }
}
