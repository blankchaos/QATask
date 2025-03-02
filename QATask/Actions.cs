using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace QATask;

public class Actions(IWebDriver driver, Waits waits)
{

    public void Click(string locator, int timeoutSeconds = 30, Waits.SupportedWaits? supportedWait = Waits.SupportedWaits.Clickable)
    {
        waits.WaitForElement(locator, timeoutSeconds, supportedWait);
        driver.FindElement(By.CssSelector(locator)).Click();
    }

    public void ClickIfFound(string locator, int timeoutSeconds = 30, Waits.SupportedWaits? supportedWait = Waits.SupportedWaits.Clickable)
    {
        waits.WaitForElement(locator, timeoutSeconds, supportedWait);
        try
        {
            driver.FindElement(By.CssSelector(locator)).Click();
        }
        catch (ElementNotVisibleException e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public void EnterText(string locator, string text)
    {
        waits.WaitForElement(locator);
        var element = driver.FindElement(By.CssSelector(locator));
        element.Clear();
        element.SendKeys(text);
        WebDriverWait wait = new(driver, TimeSpan.FromSeconds(5));
        wait.Until(d => element.GetAttribute("value") == text);
    }

    public string GetText(string locator)
    {
        waits.WaitForElement(locator);
        return driver.FindElement(By.CssSelector(locator)).Text;
    }

    public bool IsFoundInContainer(string locator, string tagName, string text)
    {
        waits.WaitForElement(locator);
        var container = driver.FindElement(By.CssSelector(locator));
        IList<IWebElement> spanElements = container.FindElements(By.TagName(tagName));

        var containsText = spanElements.Any(span => span.Text.Trim().Equals(text, StringComparison.OrdinalIgnoreCase));

        return containsText;
    }

    public bool CheckIfExists(string locator, int timeout)
    {
        try
        {
            waits.WaitForElement(locator, timeout);
            return true;
        }
        catch (WebDriverTimeoutException)
        {
            return false;
        }
    }
}