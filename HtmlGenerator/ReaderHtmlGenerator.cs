using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MangaReaper.Models;

namespace MangaReaper.HtmlGenerator
{
    public class ReaderHtmlGenerator
    {
        private string html;

        private string headerHtml =
        "<html>" +
        "<head>" +
        "<link href=\"https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css\" rel=\"stylesheet\">" +
        "</head>" +
        "<body>";
        private string containerBefore = "<div class=\"container-fluid text-center\">";
        private string containerAfter = "</div>";
        private string footerHtml = "</body></html>";

        public void GenerateReaderHtml(string directoryPath)
        {
            ReaderDataGenerator readerDataGenerator = new ReaderDataGenerator();
            if (readerDataGenerator.TryGenerateReaderData(directoryPath, out List<Chapter> chapters))
            {
                for (int i = 0; i < chapters.Count; i++)
                {
                    html = "";
                    html += headerHtml;
                    GenerateImagesHtml(chapters[i]);
                    GenerateButtonsHtml(i, chapters.Count);
                    html += footerHtml;                    
                    WriteFileHtml(directoryPath, chapters[i]);
                }
            }
        }

        private void GenerateButtonsHtml(int currentChapter, int chaptersAmount)
        {
            html += containerBefore;
            html += "<div class\"row\">";
            if (currentChapter > 0)
            {
                GeneratePreviusButtonHtml(currentChapter);
            }
            if (currentChapter < chaptersAmount)
            {
                GenerateNextButtonHtml(currentChapter);
            }
            html += "</div";
            html += containerAfter;
        }

        private void GenerateNextButtonHtml(int currentChapter)
        {
            html += $"<a class=\"btn btn-primary btn-lg\" href=\"capitulo-{currentChapter + 1}.html\" role=\"button\">Next</a>";
        }

        private void GeneratePreviusButtonHtml(int currentChapter)
        {
            html += $"<a class=\"btn btn-primary btn-lg\" href=\"capitulo-{currentChapter - 1}.html\" role=\"button\">Previus</a>";
        }

        private void GenerateImagesHtml(Chapter chapter)
        {
            if (chapter != null && chapter.ChapterImages != null)
            {
                int chapterImagesAmount = chapter.ChapterImages.Length;
                if (chapterImagesAmount > 0)
                {
                    html += containerBefore;
                    for (int i = 0; i < chapterImagesAmount; i++)
                    {
                        string imageUrl = $"{chapter.ChapterName}/{chapter.ChapterImages[i]}";
                        html += $"<div class\"row\"><img src=\"{imageUrl}\"></div>";
                    }
                    html += containerAfter;
                }
            }
        }

        private void WriteFileHtml(string directoryPath, Chapter chapter)
        {
            try
            {
                if (chapter != null)
                {
                    var path = Path.Combine(directoryPath, $"{chapter.ChapterName}.html");
                    if (!File.Exists(path))
                    {
                        File.Create(path);
                    }
                    File.WriteAllText(path, html);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Dont generate html file - {ex}.");
            }
        }
    }
}