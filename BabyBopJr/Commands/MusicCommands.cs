using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyBopJr.Commands
{
    [Name("Music")]
    public class MusicCommands : ModuleBase<SocketCommandContext>
    {
        [Command("join")]
        [Summary("Makes the bot join the voice channel of the user who requested it.")]
        public async Task JoinCommand()
            => await Managers.AudioManager.JoinAsync(Context.Guild, Context.User as IVoiceState, Context.Channel as ITextChannel);

        [Command("play")]
        [Summary("Plays from YouTube")]
        public async Task PlayCommand([Remainder] string search)
            => await Context.Channel.SendMessageAsync(await Managers.AudioManager.PlayAsync(Context.User as SocketGuildUser, Context.Guild, search));

        [Command("leave")]
        [Summary("Leaves the voice channel")]
        public async Task LeaveCommand()
            => await Context.Channel.SendMessageAsync(await Managers.AudioManager.LeaveAsync(Context.Guild));
        [Command("stop")]
        [Summary("Stops the queue/song")]
        public async Task Stop()
            => await Context.Channel.SendMessageAsync(await Managers.AudioManager.StopAsync(Context.Guild));
        [Command("skip")]
        [Summary("Skips to the next song in the queue")]
        public async Task Skip()
           => await Context.Channel.SendMessageAsync( await Managers.AudioManager.SkipTrackAsync(Context.Guild));

    }
}
