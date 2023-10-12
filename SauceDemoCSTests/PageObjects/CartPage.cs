using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;

namespace SauceDemoCSTests.PageObjects
{
	public class CartPage
	{
		IWebDriver driver;
        By byCartPageTitle = By.XPath("//span[text()='Your Cart']");
        By byProductName = By.CssSelector("div[class='inventory_item_name']");
        By byProductPrice = By.CssSelector("div[class='inventory_item_price']");
        By byRemoveButton = By.CssSelector("button[data-test*='remove-']");
        By byProductQty = By.CssSelector("div[class='cart_quantity']");

		public CartPage(IWebDriver driver)
		{
			this.driver = driver;
			PageFactory.InitElements(driver, this);
		}

        [FindsBy(How = How.XPath, Using = "//span[text()='Your Cart']")]
        private IWebElement cartPageTitle;

        [FindsBy(How = How.XPath, Using = "//button[text()='Checkout']")]
        private IWebElement checkoutButton;

        [FindsBy(How = How.CssSelector, Using = "div[class='cart_item']")]
        private IList<IWebElement> cartItems;

        [FindsBy(How = How.CssSelector, Using = "span[class='shopping_cart_badge']")]
        private IWebElement shoppingCartBadge;

        public void WaitForPageDisplay()
        {
            WebDriverWait wait = new(driver, TimeSpan.FromSeconds(10));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(byCartPageTitle));
        }

        public CheckoutOnePage Checkout()
        {
            checkoutButton.Click();
            return new CheckoutOnePage(driver);
        }

        public IWebElement GetShoppingCartBadge()
        {
            return shoppingCartBadge;
        }

        public double GetProductPrice(String productName)
        {
            IWebElement? itemPrice = null;

            foreach (IWebElement item in cartItems)
            {
                string currentProductName = item.FindElement(byProductName).Text;

                if (currentProductName == productName)
                {
                    itemPrice = item.FindElement(byProductPrice);
                    break;
                }

            }

            if (itemPrice != null)
            {
                return double.Parse(itemPrice.Text[1..]); // Removes $ sign and converts to double to handle decimals
            } else
            {
                throw new NoSuchElementException("Product not found: " + productName);
            }
        }

        public int GetProductQty(String productName)
        {
            int prodQty = 0;

            foreach(IWebElement item in cartItems)
            {
                string currentProductName = item.FindElement(byProductName).Text;

                if (currentProductName == productName)
                {
                    prodQty = int.Parse(item.FindElement(byProductQty).Text);
                    break;
                }
            }

            return prodQty;
        }

        public void RemoveProduct(String productName)
        {
            foreach(IWebElement item in cartItems)
            {
                string currentProductName = item.FindElement(byProductName).Text;

                if (currentProductName == productName)
                {
                    item.FindElement(byRemoveButton).Click();
                    break;
                }
            }
        }
    }
}

