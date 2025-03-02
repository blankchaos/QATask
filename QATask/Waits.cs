using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace QATask;

public class Waits(IWebDriver driver)
{
    public enum SupportedWaits
    {
        Visible,
        Clickable,
        Invisible
    }

    public void WaitForElement(string locator, int timeoutSeconds = GlobalVars.DefaultWaitInSeconds,
        SupportedWaits? method = SupportedWaits.Visible)
    {
        var wait = DefaultWait(timeoutSeconds);
        Console.WriteLine($"Waiting for element {locator} to be {method.ToString()}");
        switch (method)
        {
            case SupportedWaits.Clickable:
                wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(locator)));
                wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(locator)));
                break;
            case SupportedWaits.Visible:
                wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(locator)));
                break;
            case SupportedWaits.Invisible:
                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.CssSelector(locator)));
                break;
            default:
                wait.Until(x => x.FindElement(By.CssSelector(locator)));
                break;
        }
    }

    public void WaitForElementToDisappear(string locator, int timeoutSeconds = GlobalVars.DefaultWaitInSeconds)
    {
        var wait = DefaultWait(timeoutSeconds);
        Console.WriteLine($"Waiting for element {locator} to disappear");
        wait.Until(drv =>
        {
            try
            {
                var element = drv.FindElement(By.CssSelector(locator));
                return element.Displayed == false;
            }
            catch (NoSuchElementException)
            {
                return true;
            }
        });
    }

    public void WaitForAttributeToMatchValue(IWebElement element, string attribute, string value)
    {
        var wait = DefaultWait(GlobalVars.DefaultWaitInSeconds);
        wait.Until(d => element.GetAttribute(attribute) == value);
    }

    private DefaultWait<IWebDriver> DefaultWait(int timeoutSeconds)
    {
        DefaultWait<IWebDriver> wait = new(driver)
        {
            Timeout = TimeSpan.FromSeconds(timeoutSeconds),
            PollingInterval = TimeSpan.FromMilliseconds(250)
        };
        wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
        return wait;
    }
}