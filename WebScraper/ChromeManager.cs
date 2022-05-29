using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace WebScraper
{
    public class ChromeManager
    {
        public IWebDriver Driver { get; set; }

        /// <summary>
        /// Creates an instance of a selenium Chrome driver
        /// </summary>
        /// <param name="url">Optional URL to navigate to on creation</param>
        /// <param name="arguments">Optional Chrome arguments (i.e. --headless --disable-gpu)(For testing use "start-maximized")</param>
        /// <param name="pageLoadStrategy">Default, Eager, Normal, None (For testing use "Eager"</param>
        public ChromeManager(string url="", string arguments = "--headless --disable-gpu", PageLoadStrategy pageLoadStrategy = PageLoadStrategy.Default)
        {
            //Set options for the driver with optional parameters
            var options = new ChromeOptions()
            {
                PageLoadStrategy = pageLoadStrategy
            };
            options.AddArguments(arguments);
            //options.AddArguments("--headless --disable-gpu");

            //Create driver
            Driver = new ChromeDriver(options);

            //Go to url if one is specified
            if (!string.IsNullOrEmpty(url) && url != "") Driver.Navigate().GoToUrl(url);

        }
    }
}
