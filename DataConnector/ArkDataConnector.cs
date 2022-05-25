using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft;
using Newtonsoft.Json.Linq;
using System.IO;



namespace DataConnector
{
    public static class ArkDataConnector
    {
        /// <summary>
        /// matches an unformatted taming-foodsource to an accepted, formatted version (i.e. "mutton" = "raw mutton")
        /// </summary>
        /// <param name="kibble"></param>
        /// <returns>formatted food source</returns>
        public static string MatchKibbleFromInput(string kibble)
        {
            //Read the JSON File
            JObject arkVarJSON = JObject.Parse(File.ReadAllText(@"Data\ArkVar.json"));

            //Create an array from JSON results
            JArray kibbles = (JArray)arkVarJSON["kibble"];
            JArray carnivoreFood = (JArray)arkVarJSON["carnivorefood"];
            JArray herbivoreFood = (JArray)arkVarJSON["herbivorefood"];

            //Convert to string array
            string[] kibbleStrArray = kibbles.ToObject<string[]>();
            string[] carnivoreArray = carnivoreFood.ToObject<string[]>();
            string[] herbivoreArray = herbivoreFood.ToObject<string[]>();

            //Sort for 1 result only
         
            var kibbleResult= kibbleStrArray.Where(x => x.Contains($"{kibble.ToLower()}")).FirstOrDefault() ?? 
                carnivoreArray.Where(x => x.Contains($"{kibble.ToLower()}")).FirstOrDefault() ??
                herbivoreArray.Where(x => x.Contains($"{kibble.ToLower()}")).FirstOrDefault();

            return kibbleResult;
        }

        /// <summary>
        /// Creates an instance of the web driver, then gathers the data
        /// on the dinosaur
        /// </summary>
        /// <param name="creature"></param>
        /// <returns>Formatted String for Discord</returns>
        public static async Task<string> GetCreatureTame(Data.ArkData.Creature creature)
        {
            WebScraper.ArkWikiConnector arkWikiConnector = new WebScraper.ArkWikiConnector();
            string[] inputData = new string[]
            {
                FormatURL(creature),
                creature.CreatureLevel,
                creature.TamingSettings.TamingSpeed,
                creature.TamingSettings.FoodDrainMultiplier
            };
            string[] dinoData = await arkWikiConnector.GetTameWithWebScraper(inputData);
            return FormatAsMessageResponse(dinoData);
        }


        //Creates a formatted message out of the gathered data
        private static string FormatAsMessageResponse(string[] DinoInfo)
        {
            throw new NotImplementedException();
        }

        private static string FormatURL(Data.ArkData.Creature creature)
        {
            return $"https://www.dododex.com/taming/{creature.CreatureName}";
        }
    }
}
