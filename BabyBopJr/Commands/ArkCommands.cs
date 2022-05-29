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
    [Name("Ark")]
    public class ArkCommands : ModuleBase<SocketCommandContext>
    {
        [Command("tame")]
        [Summary("Gives taming stats on a dinosaur of a particular level, food, and server settings")]
        public async Task TameDino([Remainder] string arguments)
            => await Context.Channel.SendMessageAsync(await Managers.DinoManager.TameAsync(arguments));
    }
}
