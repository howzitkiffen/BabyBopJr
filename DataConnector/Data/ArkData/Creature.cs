using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataConnector.Data.ArkData
{
    public class Creature
    {
        public string CreatureName { get; set; }
        public string CreatureLevel { get; set; }
        public IEnumerable<Food> FoodUsed { get; set; }
        public TamingSettings TamingSettings { get; set; } = new TamingSettings();


        public Creature(string inputName, string creatureLevel)
        {
            CreatureName = GetCreatureByName(inputName);
            CreatureLevel = creatureLevel;
        }

        private string GetCreatureByName(string inputName)
        {
            var logFile = File.ReadAllLines(("DinoList.txt"));
            var Dinos = new List<string>(logFile);
            return Dinos.Where(x => x.Contains($"{inputName.ToLower()}")).FirstOrDefault();
        }

    }
}
