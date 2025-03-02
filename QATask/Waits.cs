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
    
    public void WaitForElement(string locator, int timeoutSeconds = 30, SupportedWaits? method = SupportedWaits.Visible)
    {
        DefaultWait<IWebDriver> wait = new(driver)
        {
            Timeout = TimeSpan.FromSeconds(timeoutSeconds),
            PollingInterval = TimeSpan.FromMilliseconds(250)
        };
        wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
        switch (method)
        {
            case SupportedWaits.Clickable:
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
    
    public void WaitForElementToDisappear(string locator, int timeoutSeconds = 30)
    {
        DefaultWait<IWebDriver> wait = new(driver)
        {
            Timeout = TimeSpan.FromSeconds(timeoutSeconds),
            PollingInterval = TimeSpan.FromMilliseconds(250)
        };
        wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
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

    public void ExplicitWait(string locator, int seconds)
    {
        WebDriverWait wait = new(driver, TimeSpan.FromSeconds(seconds));
        var selector = driver.FindElement(By.CssSelector(locator)); 
        wait.Until(d => selector.Displayed);
        //this is to avoid using Thread.Sleep when there's no other way
    }
}