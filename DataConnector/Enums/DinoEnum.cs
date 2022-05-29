using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataConnector
{
    public static class DinoEnum
    {
        private static IEnumerable<IWebElement> unformattedDinos { get; set; }
        public static List<string> Dinos { get; set; }

        public static void GetListOfDinos()
        {
            // Create a driver instance for chromedriver
            // Set driver options
            //Go to the wiki
            WebScraper.ChromeManager chromeManager = new WebScraper.ChromeManager("https://ark.fandom.com/wiki/Creatures");
            IWebDriver driver = chromeManager.Driver;

            //Get List of All Creatures via xpath
            unformattedDinos = driver.FindElements(By.XPath(".//tbody//tr//td//a"));

            //Remove any entries with unwanted characters, numbers etc.
            try
            {
                IEnumerable<string> tempEnumerable = (from t in unformattedDinos
                    where t.Text != null && t.Text != "" && t.Text != "Icon" && t.Text != "Text" && !t.Text.All(char.IsDigit)
                    select t.Text.ToLower());
                Dinos = tempEnumerable.ToList();

                System.IO.File.WriteAllLines("DinoList.txt", Dinos);

                driver.Quit();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
    }
}
