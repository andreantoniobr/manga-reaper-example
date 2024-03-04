using MangaReaper.Models;

namespace MangaReaper.HtmlGenerator
{
    public class ReaderDataGenerator
    {
        public bool TryGenerateReaderData(string directoryPath, out List<Chapter> chapters)
        {
            chapters = new List<Chapter>();
            bool canGenerateReaderData = false;
            if (TryGetDirectorys(directoryPath, out string[] directorys))
            {
                for (int i = 0; i < directorys.Length; i++)
                {
                    if (TryGetFilesInDirectory(directorys[i], out string[] fileNames) &&
                        TryGetDirectoryName(directorys[i], out string directoryName))
                    {
                        Chapter chapter = new Chapter(directoryName, fileNames);
                        if (chapters != null && chapter != null)
                        {
                            canGenerateReaderData = true;
                            chapters.Add(chapter);
                        }
                    }
                }
            }
            return canGenerateReaderData;
        }

        private bool TryGetFilesInDirectory(string directoryPath, out string[] fileNames)
        {
            fileNames = null;
            bool canGetFilesInDirectory = false;
            try
            {
                string[] filesPath = Directory.GetFiles(directoryPath);
                if (filesPath != null)
                {
                    int filesPathAmount = filesPath.Length;
                    if (filesPathAmount > 0)
                    {
                        fileNames = new string[filesPathAmount];
                        if (fileNames != null)
                        {
                            for (int i = 0; i < filesPathAmount; i++)
                            {
                                fileNames[i] = Path.GetFileName(filesPath[i]);
                            }
                            if (fileNames.Length > 0)
                            {
                                canGetFilesInDirectory = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string error = $"Dont Get Files In Directory - {ex}";
                Console.WriteLine(error);
            }
            return canGetFilesInDirectory;
        }

        private bool TryGetDirectorys(string directoryPath, out string[] directorys)
        {
            directorys = null;
            bool canGetFilesInDirectory = false;
            try
            {
                directorys = Directory.GetDirectories(directoryPath);
                if (directorys != null && directorys.Length > 0)
                {
                    canGetFilesInDirectory = true;
                }
            }
            catch (Exception)
            {
            }
            return canGetFilesInDirectory;
        }

        private bool TryGetDirectoryName(string directoryPath, out string directoryName)
        {
            directoryName = null;
            bool canGetDirectoryName = false;
            try
            {
                directoryName = Path.GetFileName(directoryPath);
                canGetDirectoryName = true;
            }
            catch (Exception)
            {
            }
            return canGetDirectoryName;
        }
    }
}