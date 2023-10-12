using SauceDemoCSTests.PageObjects;
using SauceDemoCSTests.Utilities;

namespace SauceDemoCSTests;

// Run all tests in this class in parallel
[Parallelizable(ParallelScope.Children)]
public class Tests : Base
{
    [Test, TestCaseSource(nameof(AddValidLoginTestData)), Category("Login")]
    public void SuccessfulLogin(String username, String password)
    {

        LoginPage loginPage = new LoginPage(getDriver());

        DashboardPage dashboardPage = loginPage.Login(username, password);

        dashboardPage.WaitForPageDisplay();

        string dashboardPageTitle = dashboardPage.GetPageTitle().Text;
        string expectedTitle = "Products";

        StringAssert.Contains(expectedTitle, dashboardPageTitle);
    }

    [Test, TestCaseSource(nameof(AddInvalidLoginTestData)), Category("Login")]
    public void FailedLogin(String wrongUsername, String wrongPassword)
    {
        LoginPage loginPage = new LoginPage(getDriver());

        loginPage.Login(wrongUsername, wrongPassword);

        string errorMessage = loginPage.GetErrorMessage().Text;
        string expectedErrorMessage = "Epic sadface: Username and password do not match any user in this service";

        StringAssert.Contains(expectedErrorMessage, errorMessage);
    }
    
    [Test, TestCaseSource(nameof(AddSubmitOrderTestData)), Category("ShoppingCart")]
    public void SubmitOrder(String username, String password, String productName)
    {
        LoginPage loginPage = new LoginPage(getDriver());

        DashboardPage dashboardPage = loginPage.Login(username, password);
        dashboardPage.WaitForPageDisplay();

        dashboardPage.AddProductToCart(productName);
        CartPage cartPage = dashboardPage.ClickShoppingCart();

        CheckoutOnePage checkoutOnePage = cartPage.Checkout();

        string firstName = FactoryData.GetRandomFirstName();
        string lastName = FactoryData.GetRandomLastName();
        string postalCode = FactoryData.GetRandomPostalCode();

        checkoutOnePage.FillCheckoutInfo(firstName, lastName, postalCode);

        CheckoutTwoPage checkoutTwoPage = checkoutOnePage.ClickContinue();
        CheckoutCompletePage checkoutCompletePage = checkoutTwoPage.ClickFinish();

        checkoutCompletePage.WaitForPageDisplay();

        string currentCompleteHeader = checkoutCompletePage.GetCompleteHeader().Text;
        string expectedCompleteHeader = "Thank you for your order!";

        StringAssert.Contains(expectedCompleteHeader, currentCompleteHeader);

        checkoutCompletePage.ClickMenuButton();
        loginPage = checkoutCompletePage.Logout();

        string currentLogoTitle = loginPage.GetLogoTitle().Text;
        string expectedLogoTitle = "Swag Labs";

        StringAssert.Contains(expectedLogoTitle, currentLogoTitle);
    }

    [Test, TestCaseSource(nameof(AddMultipleScenariosTestData)), Category("ShoppingCart")]
    public void MultipleScenarios(String username, String password, String filterOption, String sauceLabsFleeceJacketName, String sauceLabsOnesieName)
    {

        //1. Login to https://www.saucedemo.com/
        LoginPage loginPage = new LoginPage(getDriver());
        DashboardPage dashboardPage = loginPage.Login(username, password);
        dashboardPage.WaitForPageDisplay();

        //2. Change the Product Sort to “Price (low to high)” on “Products
        dashboardPage.SelectOption(filterOption);

        //3. Assert if the Selected(Displayed) Item on the Product Sort is “Price(low to high)”
        string expectedOptionText = filterOption;
        string currentOptionText = dashboardPage.GetSelectedOption().Text;

        Assert.That(currentOptionText, Is.EqualTo(expectedOptionText));

        //4. Capture all prices from the product page and Assert if it is in ascending order.
        double[] pricesList = dashboardPage.GetAllPrices();

        Assert.That(dashboardPage.IsOrderedFromLowToHigh(pricesList), Is.True, "Prices are not ordered from low to high");

        //5. Click “Add to Cart” button for “Sauce Labs Fleece Jacket” and “Sauce Labs Onesie”
        dashboardPage.AddProductToCart(sauceLabsFleeceJacketName);
        dashboardPage.AddProductToCart(sauceLabsOnesieName);

        Assert.Multiple(() =>
        {
            Assert.That(dashboardPage.IsRemoveButtonDisplayed(sauceLabsFleeceJacketName), Is.True);
            Assert.That(dashboardPage.IsRemoveButtonDisplayed(sauceLabsOnesieName), Is.True);
        });

        //7. Capture price of “Sauce Labs Fleece Jacket” from “Products” page
        double sauceLabsFleeceJacketPrice = dashboardPage.GetPrice(sauceLabsFleeceJacketName);

        //8. Capture price of “Sauce Labs Onesie” from “Products” page
        double sauceLabsOnesiePrice = dashboardPage.GetPrice(sauceLabsOnesieName);

        //9. Capture value from “Cart Icon” on the top right and assert is its “2”.
        string currentShoppingCartBadgeText = dashboardPage.GetShoppingCartBadge().Text;
        string expectedCartBadgeText = "2";

        Assert.That(expectedCartBadgeText, Is.EqualTo(currentShoppingCartBadgeText));

        //10. Click “Cart” icon
        CartPage cartPage = dashboardPage.ClickShoppingCart();

        //11. Capture price of “Sauce Labs Fleece Jacket” from “Your Cart” page
        double sauceLabsFleeceJacketCartPrice = cartPage.GetProductPrice(sauceLabsFleeceJacketName);

        //12. Capture price of “Sauce Labs Onesie” from “Your Cart“ page
        double sauceLabsOnesieCartPrice = cartPage.GetProductPrice(sauceLabsOnesieName);

        Assert.Multiple(() =>
        {
            //13. Assert price values from step 7 and step 11
            Assert.That(sauceLabsFleeceJacketPrice, Is.EqualTo(sauceLabsFleeceJacketCartPrice));

            //14. Assert price values from step 8 and step 12
            Assert.That(sauceLabsOnesiePrice, Is.EqualTo(sauceLabsOnesieCartPrice));
        });

        //15. Click “Remove” button for “Sauce Labs Onesie” on “Your Cart“ page
        cartPage.RemoveProduct(sauceLabsOnesieName);

        //16. Capture quantity of “Sauce Labs Fleece Jacket” from “Your Cart“page
        int sauceLabsFleeceJacketCartQty = cartPage.GetProductQty(sauceLabsFleeceJacketName);

        //17. Capture value from “Cart Icon” on the top right from “Your Cart“page
        string shoppingCartBadge = cartPage.GetShoppingCartBadge().Text;

        //18. Assert quantity values from step 16 and step 17
        int expectedSauceLabsFleeceJacketQty = 1;
        string expectedShoppingCartBadgeText = "1";

        Assert.Multiple(() =>
        {
            Assert.That(sauceLabsFleeceJacketCartQty, Is.EqualTo(expectedSauceLabsFleeceJacketQty));
            Assert.That(shoppingCartBadge, Is.EqualTo(expectedShoppingCartBadgeText));
        });

        //19. Click “Checkout” button on “Your Cart“ page
        CheckoutOnePage checkoutOnePage = cartPage.Checkout();

        //20. Fill the “Checkout: Your Information” page (Random data).
        string firstName = FactoryData.GetRandomFirstName();
        string lastName = FactoryData.GetRandomLastName();
        string postalCode = FactoryData.GetRandomPostalCode();

        checkoutOnePage.FillCheckoutInfo(firstName, lastName, postalCode);

        //21. Click “Continue” button
        CheckoutTwoPage checkoutTwoPage = checkoutOnePage.ClickContinue();

        //22. Capture “Item total” from “Checkout: Overview” page and Assert it with price from step 7
        double itemTotal = double.Parse(checkoutTwoPage.GetItemTotalPrice().Text.Split("$")[1]);

        //23. Click “Finish” button on “Checkout: Overview” page
        CheckoutCompletePage checkoutCompletePage = checkoutTwoPage.ClickFinish();

        //24. Capture “Thank you for your order” text from “Checkout Complete” page and Assert it
        string completeHeaderText = checkoutCompletePage.GetCompleteHeader().Text;
        string expectedCompleteHeaderText = "Thank you for your order!";

        Assert.That(expectedCompleteHeaderText, Is.EqualTo(completeHeaderText));

        //25. Click “Menu” Icon on top left of the header
        checkoutCompletePage.ClickMenuButton();

        //26. Click “Logout” button
        checkoutCompletePage.Logout();

        // Assert correct Logout
        string currentLogoTitle = loginPage.GetLogoTitle().Text;
        string expectedLogoTitle = "Swag Labs";

        StringAssert.Contains(expectedLogoTitle, currentLogoTitle);
    }

    public static IEnumerable<TestCaseData> AddValidLoginTestData()
    {
        yield return new TestCaseData(GetDataParser().ExtractData("username"), GetDataParser().ExtractData("password"));
    }

    public static IEnumerable<TestCaseData> AddInvalidLoginTestData()
    {
        yield return new TestCaseData(GetDataParser().ExtractData("wrong_username"), GetDataParser().ExtractData("wrong_password"));
    }

    public static IEnumerable<TestCaseData> AddSubmitOrderTestData()
    {
        yield return new TestCaseData(
                    GetDataParser().ExtractData("username"),
                    GetDataParser().ExtractData("password"),
                    GetDataParser().ExtractData("product_name"));
    }

    public static IEnumerable<TestCaseData> AddMultipleScenariosTestData()
    {
        yield return new TestCaseData(
                    GetDataParser().ExtractData("username"),
                    GetDataParser().ExtractData("password"),
                    GetDataParser().ExtractData("filter_option"),
                    GetDataParser().ExtractData("fleece_jacket_name"),
                    GetDataParser().ExtractData("onesie_name")
                    );
    }
}