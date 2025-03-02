namespace QATask.PageObjects;

public class WallpapersPage(Actions actions) : DriverInitialization
{
    private const string FirstFoundItem = ".aspect-wallpaper";

    public void OpenFirstWallpaper()
    {
        actions.Click(FirstFoundItem);
    }
}