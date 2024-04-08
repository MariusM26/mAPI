using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using UiTests;

namespace UITests;

public class WebDriverProvider
{
    #region Public Methods
    public static IWebDriver Get() => SetDriverWait(GetChromeDriver());
    #endregion


    #region Private Methods
    private static IWebDriver GetChromeDriver()
    {
        var chromeOptions = new ChromeOptions
        {
            AcceptInsecureCertificates = true,
        };

        chromeOptions.AddArguments("--incognito");
        chromeOptions.AddArgument("--start-maximized");
        chromeOptions.AddArguments("--no-sandbox");
        chromeOptions.AddArguments("--disable-dev-shm-usage");

        return new ChromeDriver(chromeOptions);
    }

    private static IWebDriver SetDriverWait(IWebDriver driver)
    {
        driver.Manage().Timeouts().PageLoad = WaitPeriods.PageLoad;
        driver.Manage().Timeouts().ImplicitWait = WaitPeriods.ImplicitWait;
        driver.Manage().Timeouts().AsynchronousJavaScript = WaitPeriods.ExplicitWait;

        return driver;
    }
    #endregion
}