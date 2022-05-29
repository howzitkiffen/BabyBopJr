using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataConnector.Data.ArkData
{
    public class Food
    {
        public string FoodName { get; set; }
        public string NumberNeeded { get; set; }
        public Effectiveness TamingEffectiveness { get; set; }
    }
}
