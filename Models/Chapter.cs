using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MangaReaper.Models
{
    public class Chapter
    {
        private string _chapterName;
        private string[] _chapterImages;

        public string ChapterName => _chapterName;

        public string[] ChapterImages => _chapterImages;        

        public Chapter(string chapterName, string[] chapterImages)
        {
            _chapterName = chapterName;
            _chapterImages = chapterImages;
        }
    }
}