using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;

namespace SauceDemoCSTests.PageObjects
{
	public class DashboardPage
	{
		private IWebDriver driver;

        By byPageTitle = By.CssSelector(".title");
        By byProductCardName = By.CssSelector("a div[class='inventory_item_name']");
        By byAddToCartButton = By.CssSelector("button[class*='btn_primary']");
        By byProductPrice = By.CssSelector("div[class='inventory_item_price']");
        By byRemoveButton = By.XPath("//button[contains(@data-test, 'remove-')]");

        public DashboardPage(IWebDriver driver)
		{
            this.driver = driver;
            PageFactory.InitElements(driver, this);
        }

        [FindsBy(How = How.CssSelector, Using = ".title")]
        private IWebElement pageTitle;

        [FindsBy(How = How.CssSelector, Using = "div[class='inventory_item']")]
        private IList<IWebElement> productCards;

        [FindsBy(How = How.CssSelector, Using = "a[class='shopping_cart_link']")]
        private IWebElement shoppingCartLink;

        [FindsBy(How = How.CssSelector, Using = "select[data-test='product_sort_container']")]
        private IWebElement sortDropdown;

        [FindsBy(How = How.CssSelector, Using = "span[class='shopping_cart_badge']")]
        private IWebElement shoppingCartBadge;


        public IWebElement GetPageTitle()
        {
            return pageTitle;
        }

        public void WaitForPageDisplay()
        {
            WebDriverWait wait = new(driver, TimeSpan.FromSeconds(10));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(byPageTitle));
        }

        public void AddProductToCart(String productName)
        {
            foreach (IWebElement prodCard in productCards)
            {
                String currentProductName = prodCard.FindElement(byProductCardName).Text;

                if (currentProductName == productName)
                {
                    prodCard.FindElement(byAddToCartButton).Click();
                    break;
                }
            }
        }

        public double GetPrice(String productName)
        {
            double prodPriceNum = 0.00;

            foreach (IWebElement prodCard in productCards)
            {
                String currentProductName = prodCard.FindElement(byProductCardName).Text;

                if (currentProductName == productName)
                {
                    string prodPriceString = prodCard.FindElement(byProductPrice).Text;
                    prodPriceNum = double.Parse(prodPriceString[1..]); // Removes the $ sign
                    break;
                }
            }

            return prodPriceNum;
        }

        public bool IsRemoveButtonDisplayed(String productName)
        {
            foreach (IWebElement prodCard in productCards)
            {
                String currentProductName = prodCard.FindElement(byProductCardName).Text;

                if (currentProductName == productName)
                {
                    IWebElement removeButton = prodCard.FindElement(byRemoveButton);
                    if (removeButton != null)
                    {
                        return true;
                    } 
                }
            }
            return false; // If foreach does not return a value, remove button is not present
        }

        public CartPage ClickShoppingCart()
        {
            shoppingCartLink.Click();
            return new CartPage(driver);
        }

        public void SelectOption(String filterOption)
        {
            try
            {
                GetSelect(sortDropdown).SelectByText(filterOption);
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine($"The option '{filterOption}' was not found in the dropdown.");
            }
        }

        public IWebElement GetShoppingCartBadge()
        {
            return shoppingCartBadge;
        }

        public IWebElement GetSelectedOption()
        {
            return GetSelect(sortDropdown).SelectedOption;
        }

        public SelectElement GetSelect(IWebElement dropdown)
        {
            return new SelectElement(dropdown);
        }

        public double[] GetAllPrices()
        {
            List<double> pricesList = new List<double>();

            foreach (IWebElement prodCard in productCards)
            {
                string prodPriceString = prodCard.FindElement(byProductPrice).Text;
                double prodPriceNum = double.Parse(prodPriceString[1..]); // Removes the $ sign

                pricesList.Add(prodPriceNum);
            }

            return pricesList.ToArray();
        }

        public bool IsOrderedFromLowToHigh(double[] numbers)
        {
            if (numbers.Length <= 1)
            {
                return true; // A single element or an empty array is considered ordered
            }

            for (int i = 1; i < numbers.Length; i++)
            {
                if (numbers[i] < numbers[i - 1])
                {
                    return false; // If any following number is less than the previous one, it's not ordered
                }
            }

            return true;
        }

    }

}

