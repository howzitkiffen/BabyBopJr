using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Victoria;
using Microsoft.Extensions.DependencyInjection;
using Victoria.EventArgs;
using Discord;
using Discord.WebSocket;
using Victoria.Enums;



namespace BabyBopJr.Managers
{
    public static class AudioManager
    {
        private static readonly LavaNode _lavaNode = ServiceManager.Provider.GetRequiredService<LavaNode>();
        private static Boolean skipped = false;

        public static async Task<string> JoinAsync(IGuild guild, IVoiceState voiceState, ITextChannel Channel)
        {
            if (_lavaNode.HasPlayer(guild))
            {
                return "I'm already connected to a voice channel!";
            }

            if (voiceState.VoiceChannel is null)
            {
                return "You must be connected to a voice channel!";
            }

            try
            {
                await _lavaNode.JoinAsync(voiceState.VoiceChannel,  Channel);
                return  $"Joined {voiceState.VoiceChannel.Name}";
            }
            catch (Exception ex)
            {
                return $"Error\n{ex.Message}";
            }
        }

        public static async Task TrackEnded(TrackEndedEventArgs args)
        {
            //if (!args.Reason.ShouldPlayNext())
            //{
            //    return;
            //}
            if (skipped == false) 
            { 
                if (!args.Player.Queue.TryDequeue(out var queueable))
                {
                    //await args.Player.TextChannel.SendMessageAsync("Playback Finished.");
                    return;
                }

                if(!(queueable is LavaTrack track))
                {
                    await args.Player.TextChannel.SendFileAsync("Next item in the queue isn't a track");
                    return;
                }

                await args.Player.PlayAsync(track);
                await args.Player.TextChannel.SendMessageAsync($"Now Playing {track.Title} By {track.Author}");
            }
            else
            {
                skipped = false;
            }
        }


        //Runs when the user requests a song to play
        public static async Task<string> PlayAsync(SocketGuildUser user, IGuild guild, string query)
        {
            if (user.VoiceChannel is null)
            {
                return "You Must First Join a Voice Channel.";
            }

            if (!_lavaNode.HasPlayer(guild))
            {
                return "I'm not connected to a voice channel.";
            }

            try
            {
                //Get the player for that guild.
                var player = _lavaNode.GetPlayer(guild);

                //Find The Youtube Track the User requested.
                LavaTrack track;

                var search = Uri.IsWellFormedUriString(query, UriKind.Absolute) 
                  ? await _lavaNode.SearchAsync(Victoria.Responses.Search.SearchType.YouTube, query)
                  : await _lavaNode.SearchYouTubeAsync(query);

                //If we couldn't find anything, tell the user.
                if (search.Status == Victoria.Responses.Search.SearchStatus.NoMatches)
                {
                    return $"Dude... {query} doesn't fucking exist.  Get outta here with that shit.";
                }

                //Get the first track from the search results.
                //TODO: Add a 1-5 list for the user to pick from. (Like Fredboat)
                track = search.Tracks.FirstOrDefault();

                //If the Bot is already playing music, or if it is paused but still has music in the playlist, Add the requested track to the queue.
                if (player.Track != null && player.PlayerState is PlayerState.Playing || player.PlayerState is PlayerState.Paused)
                {
                    player.Queue.Enqueue(track);
                    Console.WriteLine($"{track.Title} has been added to the music queue.");
                    return $"{track.Title} has been added to queue.";
                }

                //Player was not playing anything, so lets play the requested track.
                await player.PlayAsync(track);
                Console.WriteLine($"Bot Now Playing: {track.Title}\nUrl: {track.Url}");
                return $"Now Playing: {track.Title}\nUrl: {track.Url}";
            }

            //If there is still a problem send a message
            catch (Exception ex)
            {
                return $"Error:\t{ex.Message}";
            }
        }

        /*This is ran when a user uses the command Leave.
            Task Returns an Embed which is used in the command call. */
        public static async Task<string> LeaveAsync(IGuild guild)
        {
            try
            {
                //Get The Player Via GuildID.
                var player = _lavaNode.GetPlayer(guild);

                //if The Player is playing, Stop it.
                if (player.PlayerState is PlayerState.Playing)
                {
                    await player.StopAsync();
                }

                //Leave the voice channel.
                await _lavaNode.LeaveAsync(player.VoiceChannel);

                Console.WriteLine($"[{DateTime.Now}]\t(AUDIO)\tBot has left.");
                return $"I've left. Thank you for playing moosik.";
            }
            //Tell the user about the error so they can report it back to us.
            catch (InvalidOperationException ex)
            {
                return ex.Message;
            }
        }

        //public static async Task<string> SkipTrackAsync(IGuild guild)
        //{
        //    try
        //    {
        //        var player = _lavaNode.GetPlayer(guild);
        //        /* Check if the player exists */
        //        if (player == null)
        //            return $"Could not aquire player.\nAre you using the bot right now? check{ConfigManager.Config.Prefix}Help for info on how to use the bot.";
        //        /* Check The queue, if it is less than one (meaning we only have the current song available to skip) it wont allow the user to skip.
        //             User is expected to use the Stop command if they're only wanting to skip the current song. */
        //        if (player.Queue.Count < 1)
        //        {
        //            return $"Unable To skip a track as there is only One or No songs currently playing." +
        //                $"\n\nDid you mean {ConfigManager.Config.Prefix}Stop?";
        //        }
        //        else
        //        {
        //            try
        //            {
        //                /* Save the current song for use after we skip it. */
        //                var currentTrack = player.Track;
        //                /* Skip the current song. */
        //                //await player.SkipAsync();
        //                await player.PauseAsync();
        //                player.Queue.TryDequeue(out var queueable);
        //                LavaTrack track = queueable;
        //                await player.PlayAsync(track);
        //                skipped = true;

        //                Console.WriteLine($"[{DateTime.Now}] Bot skipped: {currentTrack.Title}");
        //                await player.TextChannel.SendMessageAsync($"I have successfully skipped {currentTrack.Title}");
        //                return $"Now Playing {track.Title} By {track.Author}";

        //            }
        //            catch (Exception ex)
        //            {
        //                return ex.Message;
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }
        //}

        public static async Task<string> StopAsync(IGuild guild)
        {
            try
            {
                var player = _lavaNode.GetPlayer(guild);

                if (player == null)
                     await player.TextChannel.SendMessageAsync("Could not aquire player.\nAre you using the bot right now? check{GlobalData.Config.DefaultPrefix}Help for info on how to use the bot.");

                /* Check if the player exists, if it does, check if it is playing.
                     If it is playing, we can stop.*/
                if (player.PlayerState is PlayerState.Playing)
                {
                    await player.StopAsync();
                    return ("I Have stopped playback & the playlist has been cleared.");
                }

                 Console.WriteLine($"Bot has stopped playback.");
               return("I Have stopped playback & the playlist has been cleared.");
            }
            catch (Exception ex)
            {
                return  ex.Message;
            }
        }

    }
}
