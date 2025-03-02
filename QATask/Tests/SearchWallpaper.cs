using QATask.Pages;

namespace QATask.Tests;

[TestFixture]
public class SearchWallpaper : DriverInitialization
{
    /// <summary>
    ///     My preferred way of writing test, due to everything being clearly visible inside test
    /// </summary>
    [Test, Category("Web Only Tests")]
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
    [Test, Category("Web Only Tests")]
    public void IdentifyFirstWallpaper()
    {
        var wallpapersPage = new PageObjects.WallpapersPage(Actions);
        var itemsPage = new PageObjects.ItemsPage(Actions, Waits);

        OpenWallpaperSearch("/wallpapers");
        wallpapersPage.OpenFirstWallpaper();
        itemsPage.IsWallpaperPremium();
    }

    [Test, Category("Download Tests")]
    public void DownloadFreeWallpaper()
    {
        var wallpapersPage = new PageObjects.WallpapersPage(Actions);
        var itemsPage = new PageObjects.ItemsPage(Actions, Waits);

        OpenWallpaperSearch("/wallpapers?free=true");
        wallpapersPage.OpenFirstWallpaper();
        itemsPage.WaitForRelatedBlock();
        itemsPage.ClickDownloadButton();
        itemsPage.WaitForAdPopUp();
        itemsPage.WaitForDownloadToStart();
        itemsPage.WaitForDownloadToFinish();
    }

    private void OpenWallpaperSearch(string path)
    {
        Actions.ClickIfFound(LandingPage.CookiesPrimaryButton);
        Actions.NavigateToSubpage(Driver, path);
        Waits.WaitForElement(WallpapersPage.Header);
    }
}