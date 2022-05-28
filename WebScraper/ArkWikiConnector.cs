using System;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebScraper
{
    public class ArkWikiConnector
    {
        public async Task<string[]> GetTameWithWebScraper(string[] inputdata)
        {
            try
            {
                ChromeManager chromeManager = new ChromeManager(inputdata[0]);
                InputTameData(chromeManager, inputdata);
                string[] tempData = await ReadTameData(chromeManager);

                chromeManager.Driver.Quit();
                return tempData;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}\n {e.StackTrace}");

                throw;
            }
        }

        public void InputTameData( ChromeManager chromeManager, string[] inputdata)
        {
            var lvl = chromeManager.Driver.FindElement(By.Id("level"));
            lvl.Clear();
            lvl.SendKeys($"{inputdata[1]}");

            var tamingSpeed = chromeManager.Driver.FindElement(By.Id("tamingMultiplier"));
            tamingSpeed.Clear();
            tamingSpeed.SendKeys($"{inputdata[2]}");

            var foodDrainMultiplier = chromeManager.Driver.FindElement(By.Id("consumptionMultiplier"));
            foodDrainMultiplier.Clear();
            foodDrainMultiplier.SendKeys($"{inputdata[3]}");
        }

        public Task<string[]> ReadTameData(ChromeManager chromeManager)
        {
            return Task.Run(() =>
            {
                List<string> items = new List<string>();

                string rowBaseXPath= "//*[@id='tamingTable']//div[@data-ttrow]";
                string rowLeadingXpath(int number) => $"//*[@id='tamingTable']//div[@data-ttrow={number.ToString()}]";
                string KibbleXPath = "//div[@class='itemLabel white bold']";
                string AmountXPath = "//div[@class='small light button useExclusive']";
                string TimeXPath = "//div[@class='ttB3 flex1 jccenter bold']";
                string effectivePercentXPath = "//div[@class='ttB4 flex2 row tteff']//span[@class='bold']";
                string effectiveLvlXPath = "//div[@class='ttB4 flex2 row tteff']//span[@class='light']";

                IEnumerable<IWebElement> rowsElements = chromeManager.Driver
                    .FindElements(By.XPath(rowBaseXPath));

                //*[@id="tamingTable"]/div[1]/div[1]/div[1]/div
                for (int i = 0; i <= rowsElements.Count()-1; i++)
                {
                    items.Add(
                        $"{chromeManager.Driver.FindElement(By.XPath($"{rowLeadingXpath(i)}{KibbleXPath}")).Text} " +
                        $"{chromeManager.Driver.FindElement(By.XPath($"{rowLeadingXpath(i)}{AmountXPath}")).Text} {chromeManager.Driver.FindElement(By.XPath($"{rowLeadingXpath(i)}{TimeXPath}")).Text} " +
                        $"{chromeManager.Driver.FindElement(By.XPath($"{rowLeadingXpath(i)}{effectivePercentXPath}")).Text} { chromeManager.Driver.FindElement(By.XPath($"{rowLeadingXpath(i)}{effectiveLvlXPath}")).Text}"
                        );
                   
                }
                items = items.Where(item => item != " " && !string.IsNullOrEmpty(item) && item !="").ToList();
                   return items.ToArray();
            });

        }
    }
}
