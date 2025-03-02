using System.Diagnostics;
using System.Text.RegularExpressions;

namespace QATask;

public abstract partial class Downloads
{
    public static void WaitForDownloadToFinish(string fileNameWithoutExtension, int timeoutInSeconds)
    {
        var timeout = TimeSpan.FromSeconds(timeoutInSeconds);
        var stopWatch = Stopwatch.StartNew();
        var expectedFilePath = GetExpectedDownloadFilePath(fileNameWithoutExtension);

        while (stopWatch.Elapsed < timeout)
        {
            if (IsDownloadFinished(expectedFilePath))
            {
                Console.WriteLine($"Download completed: {expectedFilePath}");
                Thread.Sleep(1000); //for browser to handle that download finished
                DownloadedItemCleanup(expectedFilePath);
                return;
            }

            Console.WriteLine($"Waiting for {expectedFilePath} to finish downloading...");
            Thread.Sleep(1000);
        }

        throw new TimeoutException($"Timeout reached. File '{expectedFilePath}.jpg' not found.");
    }

    private static void DownloadedItemCleanup(string expectedFilePath)
    {
        if (File.Exists(expectedFilePath))
        {
            File.Delete(expectedFilePath);
            Console.WriteLine($"File {expectedFilePath} deleted successfully.");
        }
        else
        {
            Console.WriteLine($"File {expectedFilePath} does not exist.");
        }
    }

    private static bool IsDownloadFinished(string filePath)
    {
        if (!File.Exists(filePath)) return false;

        try
        {
            using (File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                return true;
            }
        }
        catch (IOException)
        {
            Console.WriteLine("File is still being downloaded");
        }

        return false;
    }


    private static string GetExpectedDownloadFilePath(string fileName)
    {
        var downloadPath = GetDefaultDownloadFolder();
        var sanitizedFileName = MyRegex().Replace(fileName, "_").ToLower() + ".jpg";

        return Path.Combine(downloadPath, sanitizedFileName);
    }

    private static string GetDefaultDownloadFolder()
    {
        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
    }

    [GeneratedRegex(@"[^a-zA-Z0-9]")]
    private static partial Regex MyRegex();

}