using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public class DataConnectorTest
    {
        [Test(Description = "Should return the correct food")]
        [TestCase("basic kibble", ExpectedResult = "basic kibble")]
        [TestCase("mutton", ExpectedResult = "raw mutton")]
        [TestCase("mejo", ExpectedResult ="mejoberry")]
        public string DataConnector_Data_ArkDataConnectorShould(string food)
        {
            return DataConnector.ArkDataConnector.MatchKibbleFromInput(food);
        }
    }
}
