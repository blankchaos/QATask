using System.Diagnostics;
using OpenQA.Selenium;

namespace QATask;

public class Actions(IWebDriver driver, Waits waits)
{
    public void Click(string locator, int timeoutSeconds = GlobalVars.DefaultWaitInSeconds,
        Waits.SupportedWaits? supportedWait = Waits.SupportedWaits.Clickable)
    {
        var callingMethod = new StackTrace().GetFrame(1)?.GetMethod()?.Name;
        Console.WriteLine($"Click method called from: {callingMethod}");

        waits.WaitForElement(locator, timeoutSeconds, supportedWait);
        Console.WriteLine($"Element found, clicking {locator}");
        
        driver.FindElement(By.CssSelector(locator)).Click();
    }

    public void ClickIfFound(string locator, int timeoutSeconds = GlobalVars.DefaultWaitInSeconds,
        Waits.SupportedWaits? supportedWait = Waits.SupportedWaits.Clickable)
    {
        var callingMethod = new StackTrace().GetFrame(1)?.GetMethod()?.Name;
        Console.WriteLine($"ClickIfFound method called from: {callingMethod}");

        waits.WaitForElement(locator, timeoutSeconds, supportedWait);
        try
        {
            Console.WriteLine($"Element found, clicking {locator}");
            driver.FindElement(By.CssSelector(locator)).Click();
        }
        catch (ElementNotVisibleException)
        {
            Console.WriteLine("Element not found, continuing");
        }
    }

    public void EnterText(string locator, string text)
    {
        var callingMethod = new StackTrace().GetFrame(1)?.GetMethod()?.Name;
        Console.WriteLine($"EnterText method called from: {callingMethod}");

        waits.WaitForElement(locator);
        var element = driver.FindElement(By.CssSelector(locator));
        element.Click(); //correctly trigger search bar
        element.Clear();
        element.SendKeys(text);
        waits.WaitForAttributeToMatchValue(element, attribute: "value", value: text);
    }

    public string GetText(string locator)
    {
        var callingMethod = new StackTrace().GetFrame(1)?.GetMethod()?.Name;
        Console.WriteLine($"GetText method called from: {callingMethod}");

        waits.WaitForElement(locator);
        return driver.FindElement(By.CssSelector(locator)).Text;
    }

    public bool IsFoundInContainer(string locator, string tagName, string text)
    {
        var callingMethod = new StackTrace().GetFrame(1)?.GetMethod()?.Name;
        Console.WriteLine($"IsFoundInContainer method called from: {callingMethod}");

        waits.WaitForElement(locator);
        var container = driver.FindElement(By.CssSelector(locator));
        IList<IWebElement> spanElements = container.FindElements(By.TagName(tagName));

        var containsText = spanElements.Any(span => span.Text.Trim().Equals(text, StringComparison.OrdinalIgnoreCase));

        return containsText;
    }

    public bool CheckIfExists(string locator, int timeout)
    {
        var callingMethod = new StackTrace().GetFrame(1)?.GetMethod()?.Name;
        Console.WriteLine($"CheckIfExists method called from: {callingMethod}");

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
    
    public static void NavigateToSubpage(IWebDriver driver, string path)
    {
        var url = driver.Url;

        if (path.StartsWith("/")) url += path.TrimStart("/".ToCharArray());
        driver.Navigate().GoToUrl(url);
    }
}