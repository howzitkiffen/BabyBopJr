using System;
using OpenQA.Selenium;
using System.Collections.Generic;
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

            IWebElement lvl = chromeManager.Driver.FindElement(By.Id("level"));
            lvl.SendKeys($"{inputdata[1]}");

            IWebElement tamingSpeed = chromeManager.Driver.FindElement(By.Id("tamingMultiplier"));
            tamingSpeed.SendKeys(($"{inputdata[2]}"));

            IWebElement foodDrainMultiplier = chromeManager.Driver.FindElement(By.Id("consumptionMultiplier"));
            foodDrainMultiplier.SendKeys(($"{inputdata[3]}"));
        }

        public Task<string[]> ReadTameData(ChromeManager chromeManager)
        {
            return Task.Run(() =>
            {
                List<string> items = new List<string>();
                //*[@id="tamingTable"]/div[1]/div[1]/div[1]/div
                items.Add(chromeManager.Driver
                    .FindElements(By.XPath(
                        "//*[@id='tamingTable']//div[@data-ttrow='0']//*div[@class='itemLabel white bold']/text()"))
                    .ToString());

                return items.ToArray();
            });

        }
    }
}
