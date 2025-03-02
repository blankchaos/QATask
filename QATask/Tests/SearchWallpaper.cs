using QATask.Pages;

namespace QATask.Tests;

public class SearchWallpaper : DriverInitialization
{
    /// <summary>
    ///     My preferred way of writing test, due to everything being clearly visible inside test
    /// </summary>
    [Test]
    public void SearchWallpaperByKeyword()
    {
        const string keyword = "dog";

        OpenWallpaperSearch("/wallpapers");
        Actions.EnterText(WallpapersPage.SearchInputField, keyword);
        Actions.Click(WallpapersPage.SearchButton);
        Waits.WaitForElement(WallpapersPage.TagList);
        Actions.Click(WallpapersPage.FirstFoundItem);
        var checkForKeywordInTags = Actions.IsFoundInContainer(ItemsPage.TagsContainer, "span", keyword);
        Assert.That(checkForKeywordInTags, Is.True,
            $"Expected to find element with text {keyword}, but none was found.");
    }

    /// <summary>
    ///     POM way of writing test
    /// </summary>
    [Test]
    public void IdentifyFirstWallpaper()
    {
        var wallpaperPom = new PageObjects.WallpapersPage(Actions, Waits);
        var itemPom = new PageObjects.ItemsPage(Actions, Waits);

        OpenWallpaperSearch("/wallpapers");
        wallpaperPom.OpenFirstWallpaper();
        itemPom.IsWallpaperPremium();
    }

    [Test]
    public void DownloadFreeWallpaper()
    {
        var wallpapersPage = new PageObjects.WallpapersPage(Actions, Waits);
        var itemsPage = new PageObjects.ItemsPage(Actions, Waits);

        OpenWallpaperSearch("/wallpapers?free=true");
        wallpapersPage.OpenFirstWallpaper();
        itemsPage.ClickDownloadButton();
        itemsPage.WaitForAdPopUp();
        itemsPage.WaitForDownloadToStart();
        itemsPage.WaitForDownloadToFinish();
    }

    private void OpenWallpaperSearch(string path)
    {
        Actions.ClickIfFound(LandingPage.CookiesPrimaryButton);
        Driver.NavigateToSubpage(path);
        Waits.WaitForElement(WallpapersPage.Header);
    }
}