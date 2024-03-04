using System;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;
using MangaReaper.HtmlGenerator;

async Task<bool> DownloadImageAsync(string directoryPath, string fileName, Uri uri)
{
    bool canDownloadImage = false;
    using var httpClient = new HttpClient();

    // Get the file extension
    var uriWithoutQuery = uri.GetLeftPart(UriPartial.Path);
    var fileExtension = Path.GetExtension(uriWithoutQuery);

    // Create file path and ensure directory exists
    var path = Path.Combine(directoryPath, $"{fileName}{fileExtension}");
    if (!File.Exists(path))
    {
        try
        {
            // Download the image and write to the file
            var imageBytes = await httpClient.GetByteArrayAsync(uri);
            await File.WriteAllBytesAsync(path, imageBytes);
            canDownloadImage = true;
        }
        catch (Exception)
        {
            canDownloadImage = false;
        }
    }
    return canDownloadImage;
}

var folder = "images";
int chaptersAmount = 200;
string[] imageExtensions = new string[] { "jpg", "png" };

//https://img.mangaschan.com/uploads/manga-images/s/solo-leveling/capitulo-0/2.jpg
var urlBase = "https://img.mangaschan.com/uploads/manga-images/s/solo-leveling/capitulo";

ReaderHtmlGenerator readerHtmlGenerator = new ReaderHtmlGenerator();
readerHtmlGenerator.GenerateReaderHtml(folder);

/*
for (int i = 0; i < chaptersAmount; i++)
{
    await DownloadAllChaptersImages(i);
}*/

async Task DownloadAllChaptersImages(int chapterNumber)
{
    int imageNumber = 1;
    string chapterPath = $"{folder}/capitulo-{chapterNumber}";
    if (TryCreateDiretory(chapterPath))
    {
        while (await TryDownloadChapterImage(chapterNumber, imageNumber, chapterPath))
        {
            imageNumber++;
        }
    }
}

async Task<bool> TryDownloadChapterImage(int chapterNumber, int imageNumber, string chapterPath)
{
    bool canDownloadChapterImage = false;
    var fileName = $"solo-leveling-{chapterNumber}-{imageNumber}";
    var url = $"{urlBase}-{chapterNumber}/{imageNumber}";
    if (Directory.Exists(chapterPath))
    {
        for (int i = 0; i < imageExtensions.Length; i++)
        {
            string imageUrl = $"{url}.{imageExtensions[i]}";
            if (await DownloadImageAsync(chapterPath, fileName, new Uri(imageUrl)))
            {
                canDownloadChapterImage = true;
                break;
            }
        }
    }
    return canDownloadChapterImage;
}

bool TryCreateDiretory(string directoryPath)
{
    bool canCreateDirectory = true;
    try
    {
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
    }
    catch (Exception)
    {
        canCreateDirectory = false;
    }
    return canCreateDirectory;
}