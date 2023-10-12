using System;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;

namespace SauceDemoCSTests.PageObjects
{
	public class CheckoutOnePage
	{
		IWebDriver driver;

		public CheckoutOnePage(IWebDriver driver)
		{
			this.driver = driver;
			PageFactory.InitElements(driver, this);
		}

        [FindsBy(How = How.CssSelector, Using = "input[id='first-name']")]
        private IWebElement firstNameInput;

        [FindsBy(How = How.CssSelector, Using = "input[id='last-name']")]
        private IWebElement lastNameInput;

        [FindsBy(How = How.CssSelector, Using = "input[id='postal-code']")]
        private IWebElement postalCodeInput;

        [FindsBy(How = How.CssSelector, Using = "input[id='continue']")]
        private IWebElement continueButton;


        public void FillCheckoutInfo(String firstName, String lastName, String postalCode)
		{
			firstNameInput.SendKeys(firstName);
            lastNameInput.SendKeys(lastName);
            postalCodeInput.SendKeys(postalCode);            
        }

        public CheckoutTwoPage ClickContinue()
        {
            continueButton.Click();
            return new CheckoutTwoPage(driver);
        }
    }
}

