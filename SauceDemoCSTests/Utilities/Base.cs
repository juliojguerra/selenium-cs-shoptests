using System;
using System.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using SauceDemoCSTests.Utilities;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace SauceDemoCSTests
{
    public class Base
    {
        String browserName;

        public ThreadLocal<IWebDriver> driver = new ThreadLocal<IWebDriver>();

        [SetUp]
        public void StartBrowser()
        {

            browserName = TestContext.Parameters["browserName"];

            if (browserName == null)
            {
                browserName = ConfigurationManager.AppSettings["browser"];
            }

            InitBrowser(browserName);

            new DriverManager().SetUpDriver(new ChromeConfig());
            driver.Value = new ChromeDriver();

            driver.Value.Manage().Window.Maximize();

            DotNetEnv.Env.Load();
            string url = Environment.GetEnvironmentVariable("URL");

            if (url != null)
            {
                driver.Value.Url = url;
            }
            else
            {
                Console.WriteLine("URL environment variable is not set or is null.");
            }

        }

        public IWebDriver getDriver()
        {
            return driver.Value;
        }


        public void InitBrowser(string browserName)
        {
            switch (browserName)
            {
                case "Firefox":
                    new DriverManager().SetUpDriver(new FirefoxConfig());
                    driver.Value = new FirefoxDriver();
                    break;
                case "Chrome":
                    new DriverManager().SetUpDriver(new ChromeConfig());
                    driver.Value = new ChromeDriver();
                    break;
                case "Edge":
                    new DriverManager().SetUpDriver(new EdgeConfig());
                    driver.Value = new EdgeDriver();
                    break;
            }
        }

        public static JsonReaderFile GetDataParser()
        {
            return new JsonReaderFile();
        }

        [TearDown]
        public void AfterTest()
        {
            driver.Value.Close();
            driver.Value.Quit();
        }

    } 
}

