namespace QATask.PageObjects;

public class ItemsPage(Actions actions, Waits waits)
{
    private const string DownloadButton = "div.flex-grow:nth-child(2) > button:nth-child(2)";
    private const string BadgeIcon = ".\\!pr-12";
    private const string ItemNameHeader = "h1";
    private const string DownloadTimerAdModal = "#radix-\\:r9\\:";

    public bool IsWallpaperPremium()
    {
        var premiumResultCheck = actions.CheckIfExists(BadgeIcon, timeout:5);
        Console.WriteLine(premiumResultCheck ? "Wallpaper is premium" : "Wallpaper is free");
        return premiumResultCheck;
    }
    
    public void ClickDownloadButton()
    {
        actions.Click(DownloadButton);
    }

    public void WaitForAdPopUp()
    {
        waits.WaitForElement(DownloadTimerAdModal);
    }

    public void WaitForDownloadToStart()
    {
        waits.WaitForElementToDisappear(DownloadTimerAdModal);
    }

    public void WaitForDownloadToFinish()
    {
        var fileName = actions.GetText(ItemNameHeader);
        Downloads.WaitForDownloadToFinish(fileName, 30);
    }
}