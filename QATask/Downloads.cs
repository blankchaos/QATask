namespace QATask;

public abstract class Downloads
{
    public static void WaitForDownloadToFinish(string fileNameWithoutExtension, int timeoutInSeconds)
    {
        var timeout = TimeSpan.FromSeconds(timeoutInSeconds);
        var stopWatch = System.Diagnostics.Stopwatch.StartNew();
        var expectedFilePath = GetExpectedDownloadFilePath(fileNameWithoutExtension);

        while (stopWatch.Elapsed < timeout)
        {
            if (IsDownloadFinished(expectedFilePath))
            {
                Console.WriteLine($"Download completed: {expectedFilePath}");
            }

            Console.WriteLine($"Waiting for {expectedFilePath} to finish downloading...");
            Thread.Sleep(1000);
        }

        throw new TimeoutException($"Timeout reached. File '{expectedFilePath}' not found.");
    }

    
    private static bool IsDownloadFinished(string filePath)
    {
        string[] allowedExtensions = [".jpg", ".jpeg", ".png"];

        foreach (var ext in allowedExtensions)
        {
            var fullFilePath = filePath + ext;
            if (!File.Exists(fullFilePath)) continue;
            try
            {
                using (File.Open(fullFilePath, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    return true;
                }
            }
            catch (IOException)
            {
                Console.WriteLine("File is still being downloaded");
            }
        }

        return false;
    }

    
    private static string GetExpectedDownloadFilePath(string fileName)
    {
        var downloadPath = GetDefaultDownloadFolder();
        var sanitizedFileName = fileName.Replace(" ", "_");

        return Path.Combine(downloadPath, sanitizedFileName);
    }
    
    private static string GetDefaultDownloadFolder()
    {
        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
    }
}