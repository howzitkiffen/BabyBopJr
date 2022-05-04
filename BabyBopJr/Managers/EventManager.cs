using Discord.WebSocket;
using System.Threading;
using Discord.Commands;
using System.Threading.Tasks;
using System;
using Discord;
using Victoria;
using Victoria.EventArgs;
using Microsoft.Extensions.DependencyInjection;


namespace BabyBopJr.Managers
{
    public static class EventManager
    {
        private static LavaNode _lavaNode = ServiceManager.Provider.GetRequiredService<LavaNode>();
        private static DiscordSocketClient _client = ServiceManager.GetService<DiscordSocketClient>();
        private static CommandService _commandService = ServiceManager.GetService<CommandService>();

        public static Task LoadCommands()
        {
            _client.Log += message =>
            {
                Console.WriteLine($"[{DateTime.Now}]\t({message.Message})");
                return Task.CompletedTask;
            };

            _commandService.Log += message =>
            {
                Console.WriteLine($"[{DateTime.Now}]\t({message.Message})");
                return Task.CompletedTask;
            };
            _lavaNode.OnTrackEnded += Managers.AudioManager.TrackEnded;
            _client.Ready += OnReady;
            _client.MessageReceived += OnMessageReceived;
            return Task.CompletedTask;
        }

        private static async Task OnReady()
        {

            try
            {
                await _lavaNode.ConnectAsync();
                if (_lavaNode.IsConnected)
                {
                    Console.WriteLine($"[{DateTime.Now}] Bot Connected to LavaLink");
                }
                else
                {
                    Console.WriteLine($"[{DateTime.Now}] ERROR: Bot Not Connected to LavaLink");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            Console.WriteLine($"[{DateTime.Now}] (READY) Bot is ready");
            await _client.SetStatusAsync(UserStatus.Online);
            await _client.SetGameAsync($"Prefix: {ConfigManager.Config.Prefix}", null, Discord.ActivityType.Listening);

        }

        private static async Task OnMessageReceived(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
            var context = new SocketCommandContext(_client, message);

            Console.WriteLine(message);

            if (message.Author.IsBot || message.Channel is IDMChannel) return;

            var argPos = 0;

            if (!(message.HasStringPrefix(ConfigManager.Config.Prefix, ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos))) return;

            var result = await _commandService.ExecuteAsync(context, argPos, ServiceManager.Provider);

            /* Report any errors if the command didn't execute succesfully. */
            if (!result.IsSuccess)
            {
               await context.Channel.SendMessageAsync(result.ErrorReason);
            }
            /* If everything worked fine, command will run. */
        }
    }
}
