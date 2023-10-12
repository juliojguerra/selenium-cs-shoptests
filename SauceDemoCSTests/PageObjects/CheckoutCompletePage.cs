using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;

namespace SauceDemoCSTests.PageObjects
{
	public class CheckoutCompletePage
	{
		IWebDriver driver;
		WebDriverWait wait;

        By byCompleteHeader = By.XPath("//h2[text()='Thank you for your order!']");
		By byLogoutButton = By.XPath("//a[text()='Logout']");

		public CheckoutCompletePage(IWebDriver driver)
		{
			this.driver = driver;
			PageFactory.InitElements(driver, this);
            wait = new(driver, TimeSpan.FromSeconds(10));
        }

        [FindsBy(How = How.XPath, Using = "//h2[text()='Thank you for your order!']")]
        private IWebElement completeHeader;

        [FindsBy(How = How.CssSelector, Using = "button[id='react-burger-menu-btn']")]
        private IWebElement menuButton;

        [FindsBy(How = How.XPath, Using = "//a[text()='Logout']")]
        private IWebElement logoutButton;
        

        public void WaitForPageDisplay()
		{
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(byCompleteHeader));
        }

		public void WaitForLogoutButtonDisplay()
		{
			wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(byLogoutButton));
		}

		public IWebElement GetCompleteHeader()
		{
			return completeHeader;
		}

		public void ClickMenuButton()
		{
			menuButton.Click();
		}

		public LoginPage Logout()
		{
			WaitForLogoutButtonDisplay();
            logoutButton.Click();

			return new LoginPage(driver);
		}
    }
}

