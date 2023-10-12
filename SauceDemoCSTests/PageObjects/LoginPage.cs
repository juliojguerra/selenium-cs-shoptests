using System;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;

namespace SauceDemoCSTests.PageObjects
{
	public class LoginPage
	{
		private IWebDriver driver;

		public LoginPage(IWebDriver driver)
		{
            this.driver = driver;
			PageFactory.InitElements(driver, this);
		}

        [FindsBy(How = How.CssSelector, Using = "#user-name")]
        private IWebElement usernameInput;

        [FindsBy(How = How.CssSelector, Using = "#password")]
        private IWebElement passwordInput;

        [FindsBy(How = How.CssSelector, Using = "#login-button")]
        private IWebElement loginInput;

        [FindsBy(How = How.CssSelector, Using = "h3[data-test='error']")]
        private IWebElement errorMessage;

        [FindsBy(How = How.CssSelector, Using = "div[class='login_logo']")]
        private IWebElement logoTitle;

        public DashboardPage Login(String username, String password)
        {
            usernameInput.SendKeys(username);
            passwordInput.SendKeys(password);
            loginInput.Click();

            return new DashboardPage(driver);

        }

        public IWebElement GetLogoTitle()
        {
            return logoTitle;
        }

        public IWebElement GetErrorMessage()
        {
            return errorMessage;
        }


    }
}

