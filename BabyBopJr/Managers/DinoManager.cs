using static DataConnector.ArkDataConnector;
using DataConnector.Data.ArkData;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BabyBopJr.Managers
{
    public class DinoManager
    {
        //Probably wont want this hardcoded in the future.
        private static string botMod { get; set; } = "@Howzitkiffen";


        public static async Task<string> TameAsync(string arguments)
        {
            List<string> args = arguments.Split(" ").ToList<string>();
            if (args.Count == 2)
            {
                var dino = args[0];
                var level = args[1];
                var creature = new Creature(dino, level);
                return await GetCreatureTame(creature);
            }

            return $"Something has gone wrong.  Contact {botMod} for assistance.  \n " +
                   "There may be too many items in the command, or they may be in the wrong order. \n" +
                   "The command given was " +
                   $"Creature: {args[0]}" +
                   $"Level: {args[1]}";
        }
    }
}