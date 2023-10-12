using System;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;

namespace SauceDemoCSTests.PageObjects
{
	public class CheckoutTwoPage
	{
        IWebDriver driver;

        public CheckoutTwoPage(IWebDriver driver)
		{
            this.driver = driver;
            PageFactory.InitElements(driver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "#finish")]
        private IWebElement finishButton;

        [FindsBy(How = How.CssSelector, Using = "div[class='summary_subtotal_label']")]
        private IWebElement itemTotalPrice;
        

        public CheckoutCompletePage ClickFinish()
        {
            finishButton.Click();
            return new CheckoutCompletePage(driver);
        }

        public IWebElement GetItemTotalPrice()
        {
            return itemTotalPrice;
        }
	}
}

