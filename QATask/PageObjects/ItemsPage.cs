namespace QATask.PageObjects;

public class ItemsPage(Actions actions, Waits waits) : DriverInitialization
{
    private const string DownloadButton = "div.flex-grow:nth-child(2) > button:nth-child(2)";
    private const string BadgeIcon = ".\\!pr-12";
    private const string ItemNameHeader = "h1";
    private const string DownloadTimerAdModal = "#radix-\\:r9\\:";
    private const string ItemContainer = "div[class*='CardsContainer_cards-container-items']";
    private string _fileName = "";

    public bool IsWallpaperPremium()
    {
        var premiumResultCheck = actions.CheckIfExists(BadgeIcon, 5);
        Console.WriteLine(premiumResultCheck ? "Wallpaper is premium" : "Wallpaper is free");
        return premiumResultCheck;
    }

    public void WaitForRelatedBlock()
    {
        waits.WaitForElement(ItemContainer);
    }

    public void ClickDownloadButton()
    {
        actions.Click(DownloadButton);
    }

    public void WaitForAdPopUp()
    {
        _fileName = actions.GetText(ItemNameHeader);
        waits.WaitForElement(DownloadTimerAdModal);
    }

    public void WaitForDownloadToStart()
    {
        waits.WaitForElementToDisappear(DownloadTimerAdModal);
    }

    public void WaitForDownloadToFinish()
    {
        Downloads.WaitForDownloadToFinish(_fileName, 30);
    }
}