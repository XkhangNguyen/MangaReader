using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace MangaReader.Model
{
    public class MangaModel
    {
        public int Id { get; set; }
        public string MangaTitle { get; set; } = string.Empty;
        public string MangaDescription { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string CoverImageURL { get; set; } = string.Empty;
        public List<string>? Genres { get; set; }
        public List<ChapterModel>? Chapters { get; set; }
    }

    public class JsonMangaReader
    {
        public static List<MangaModel>? ReadJsonFile(string filePath)
        {
            try
            {
                string jsonText = File.ReadAllText(filePath);
                List<MangaModel>? mangaList = JsonConvert.DeserializeObject<List<MangaModel>>(jsonText);
                return mangaList;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading JSON file: " + ex.Message);
                return null;
            }
        }
    }
}
