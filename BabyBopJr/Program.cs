using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Victoria;
using System.Xml;

namespace BabyBopJr
{
    class Program
    {
        public string GuildID { get; set; }

        static void Main(string[] args)
            => new Bot().MainAsync().GetAwaiter().GetResult();
    }


    #region Bot
    public class Bot
    {
        private DiscordSocketClient _client;
        private CommandService _commandService;

        public Bot()
        {
            _client = new DiscordSocketClient(new DiscordSocketConfig()
            {
                LogLevel = LogSeverity.Debug
            });

            _commandService = new CommandService(new CommandServiceConfig()
            {
                LogLevel = LogSeverity.Debug,
                CaseSensitiveCommands = true,
                DefaultRunMode = RunMode.Async,
                IgnoreExtraArgs = true
            });

            var collection = new ServiceCollection();

            collection.AddSingleton(_client);
            collection.AddSingleton(_commandService);
            collection.AddLavaNode(x =>
            {
                x.SelfDeaf = false;
            });
            Managers.ServiceManager.SetProvider(collection);


        }

        public async Task MainAsync()
        {
            if (string.IsNullOrWhiteSpace(Managers.ConfigManager.Config.Token)) return;

            AppDomain.CurrentDomain.ProcessExit += new EventHandler(CurrentDomain_ProcessExit);

            await RunLavaLink();

            await Managers.CommandManager.LoadCommandsAsync();
            await Managers.EventManager.LoadCommands();
            await _client.LoginAsync(TokenType.Bot, Managers.ConfigManager.Config.Token);
            await _client.StartAsync();

            await GetDinosOnStartUp();  //Fill enum with current list of creatures
            await Task.Delay(-1);
        }

        private Task RunLavaLink()
        {
            try
            {
                string thisFolder = Directory.GetCurrentDirectory();
                string LavaLinkFolder = thisFolder + @"\Java";
                string cmdText = "/C \"java -jar Lavalink.jar\"";

                var p = new Process
                {
                    StartInfo =
                {
                    FileName="C:\\Windows\\system32\\cmd.exe",
                    WorkingDirectory = LavaLinkFolder,
                    Arguments = cmdText,
                    WindowStyle=System.Diagnostics.ProcessWindowStyle.Normal
                }
                }.Start();

                return Task.CompletedTask;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
            }
            return Task.CompletedTask;
        }

        static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            Console.WriteLine("exit");
            Environment.Exit(Environment.ExitCode);
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        /// <summary>
        /// Get a list of creatures from the fan wiki and assign it to 
        /// the dinoenum
        /// </summary>
        /// <returns></returns>
        private Task GetDinosOnStartUp()
        {
            DataConnector.DinoEnum.GetListOfDinos();
            return Task.CompletedTask;
        }
    }
    #endregion
}
