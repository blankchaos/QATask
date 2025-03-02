using OpenQA.Selenium;

namespace QATask;

public static class WebDriverExtensions
{
    public static void NavigateToSubpage(this IWebDriver driver, string path)
    {
        var url = driver.Url;

        if (path.StartsWith("/")) url += path.TrimStart("/".ToCharArray());
        driver.Navigate().GoToUrl(url);
    }
}